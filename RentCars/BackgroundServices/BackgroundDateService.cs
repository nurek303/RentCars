using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RentCars.Data;
using RentCars.Models;
using RentCars.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace RentCars.BackgroundServices
{
    public class BackgroundDateService : BackgroundService
    {
        //prywatne pola, do których trzeba przypisac wartość 
        private readonly ILogger<BackgroundDateService> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly string directoryPath;
        private readonly IMapper _mapper;


        // przypisanie wartosci do pol (za kazdym razem gdy klasa jest wywołana)
        public BackgroundDateService(ILogger<BackgroundDateService> logger,
            IServiceScopeFactory factory,
            IConfiguration configuration,
            IMapper mapper)
        {
            _dbContext = factory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _logger = logger;
            directoryPath = configuration["AppSettings:ImageDir"];
            _mapper = mapper;
        }

        protected override async  Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //select z bazy 
            var dates = await _dbContext.RentalHistory
                .Include(r=>r.Reservation)
                .Include(r=>r.Car)
                .ToListAsync();      

            
            var dateNow = DateTime.Now;

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Date Service is executing");

                foreach (var date in dates)
                {
                    if (date.DateOfReturn <= dateNow.Subtract(TimeSpan.FromDays(1)))
                    {
                        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == date.Reservation.UserId);
                        var car = await _dbContext.Cars.FirstOrDefaultAsync(c => c.Id == date.Car.Id);
                        
                        if (user != null && car!=null)
                        {
                            SendEmail(user.Email, car);
                        }
                    }

                    if (date.DateOfReturn <= dateNow)
                    {
                        date.DateOfRent = null;
                        date.DateOfReturn = null;
                        _dbContext.Update(date);
                        _dbContext.SaveChanges();
                    }

                }
                await Task.Delay(TimeSpan.FromSeconds(59), stoppingToken);
            }
        }
        private void SendEmail(string recipient, Car car)
        {
            MailMessage message = new MailMessage();

            string fromMail = "justkurek71@gmail.com";
            string fromPassword = "juoxeguqkdcyywff";

            message.From = new MailAddress(fromMail);

            message.AlternateViews.Add(GetEmbeddedImage(directoryPath + car.FilePath,car.Brand , car.Model));


            message.To.Add(recipient);

            message.Subject = "Koniec wypożyczenia";

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true
            };

            smtpClient.Send(message);

        }

        private AlternateView GetEmbeddedImage(String filePath,string brand, string model)
        {
            LinkedResource res = new LinkedResource(filePath);
            res.ContentId = Guid.NewGuid().ToString();
            string htmlBody = "<h1>" + $"Rezerwacja: {brand} {model} kończy się jutro" + "</h1>" + @"<img width='600px' height='280px' src='cid:" + res.ContentId  + @"'/>" ;
                                                                       
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(res);
            return alternateView;
        }
    }
}
