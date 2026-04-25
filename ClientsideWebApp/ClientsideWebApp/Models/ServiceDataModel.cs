using Microsoft.AspNetCore.Mvc;

namespace ClientsideWebApp.Models
{
    public class ServiceDataModel
    {
        public static List<DigitalServiceModel> GetAll()
        {
            return new List<DigitalServiceModel>
            {
                new DigitalServiceModel
                {
                    Id = "1",
                    Title = "Kodulehe loomine",
                    Slug = "kodulehe-loomine",
                    ShortDescription = "Kaasaegne ja kiire veebileht sinu ettevõttele",
                    Bullets = new List<string>
                    {
                        "Toimib sujuvalt nii telefonis, tahvlis kui arvutis",
                        "SEO-sõbralik struktuur ja kood",
                        "Lihtne sisuhaldus mugava adminpaneeliga"
                    }
                },
                new DigitalServiceModel
                {
                    Id = "2",
                    Title = "E-poe loomine",
                    Slug = "epoe-loomine",
                    ShortDescription = "Müü oma tooteid internetis mugavalt",
                    Bullets = new List<string>
                    {
                        "Turvaline maksesüsteem ja tarnevalikud",
                        "Toodete kategooriad ja filtreerimine",
                        "Kohandatav disain vastavalt brändile"
                    }
                },
                new DigitalServiceModel
                {
                    Id = "3",
                    Title = "Logo disain",
                    Slug = "logo-disain",
                    ShortDescription = "Visuaalne identiteet, mis jääb meelde",
                    Bullets = new List<string>
                    {
                        "Mitmekülgne kasutus erinevates kanalites",
                        "Professionaalne ja ajatu visuaalne stiil",
                        "Kasutusvalmis lahendus nii veebis kui igapäevases turunduses"
                    }
                },
                new DigitalServiceModel
                {
                    Id = "4",
                    Title = "Raamatupidamine",
                    Slug = "raamatupidamine",
                    ShortDescription = "Korrektne ja lihtne finantshaldus",
                    Bullets = new List<string>
                    {
                        "Regulaarne aruandlus ja raportid",
                        "Maksunõustamine ja konsultatsioon",
                        "Kulude ja tulude jälgimine reaalajas"
                    }
                },
                new DigitalServiceModel
                {
                    Id = "5",
                    Title = "Konsultatsioon",
                    Slug = "konsultatsioon",
                    ShortDescription = "Selgus, suund ja konkreetsed järgmised sammud sinu veebis või äris",
                    Bullets = new List<string>
                    {
                        "Analüüs sinu olemasolevast veebist või ideest",
                        "Selge tegevusplaan ja soovitused",
                        "Abi õigete tehniliste ja disainiotsuste tegemisel"
                    }
                }
            };
        }
    }
}
