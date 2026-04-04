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
                        "Responsiivne disain igas seadmes",
                        "SEO-sõbralik struktuur ja kood",
                        "Sisuhaldus lihtsustatud adminpaneeliga"
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
                        "Mitmekülgne kasutus erinevates meediumites",
                        "Professionaalne ja ajatu stiil",
                        "Vektorformaadis failid trükiks ja veebiks"
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
                        "Maksunõu ja konsultatsioon",
                        "Kulude ja tulude jälgimine reaalajas"
                    }
                },
                new DigitalServiceModel
                {
                    Id = "5",
                    Title = "Haldus",
                    Slug = "haldus",
                    ShortDescription = "Hooldus ja tugi sinu veebile",
                    Bullets = new List<string>
                    {
                        "Veebilehe uuendused ja varukoopiad",
                        "Turvalisuse jälgimine",
                        "Kasutajatugi ja probleemide lahendamine"
                    }
                }
            };
        }
    }
}
