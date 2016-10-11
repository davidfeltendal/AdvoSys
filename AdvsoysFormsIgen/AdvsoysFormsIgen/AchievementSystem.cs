using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Toasts.Forms.Plugin.Abstractions;
using Xamarin.Forms;
using XLabs.Caching;
using XLabs.Ioc;

namespace AdvsoysFormsIgen
{
    public class Tidsreg
    {
        public const string Key = "Tidsregistreringer";

        [JsonProperty]
        public int Antal { get; set; }
    }

    public class AchievementSystem
    {
        public AchievementSystem()
        {
            MessagingCenter.Subscribe<RegistrerTidViewModel>(this, "TidRegistreret", async _ =>
            {
                var notificator = DependencyService.Get<IToastNotificator>();

                using (var cache = Resolver.Resolve<ISimpleCache>())
                {
                    var tidsregistreringer = cache.Get<Tidsreg>(Tidsreg.Key);
                    tidsregistreringer.Antal++;
                    cache.Set(Tidsreg.Key, tidsregistreringer);
                    var achievements = cache.Get<List<Achievement>>(Achievement.Key);

                    if (tidsregistreringer.Antal == 1)
                    {
                        var ach = achievements.First(a => a.Id == 1);
                        ach.Opnået = true;

                        await notificator.Notify(
                            ToastNotificationType.Success,
                            "Tillykke!",
                            "Du har foretaget din første tidsregistrering.",
                            TimeSpan.FromSeconds(5));
                    }
                    else if (tidsregistreringer.Antal == 5)
                    {
                        var ach = achievements.First(a => a.Id == 5);
                        ach.Opnået = true;

                        await notificator.Notify(
                            ToastNotificationType.Success,
                            "Tillykke!",
                            "Du har foretaget din femte tidsregistrering.",
                            TimeSpan.FromSeconds(5));
                    }
                    else if (tidsregistreringer.Antal == 10)
                    {
                        var ach = achievements.First(a => a.Id == 10);
                        ach.Opnået = true;

                        await notificator.Notify(
                            ToastNotificationType.Success,
                            "Tillykke!",
                            "Du har foretaget din 10. tidsregistrering.",
                            TimeSpan.FromSeconds(5));
                    }
                    else if (tidsregistreringer.Antal == 25)
                    {
                        var ach = achievements.First(a => a.Id == 25);
                        ach.Opnået = true;

                        await notificator.Notify(
                            ToastNotificationType.Success,
                            "Præstation låst op!",
                            "Foretag 25. tidsregistreringer.",
                            TimeSpan.FromSeconds(5));
                    }

                    cache.Set(Achievement.Key, achievements);
                }
            });
        }
    }
}