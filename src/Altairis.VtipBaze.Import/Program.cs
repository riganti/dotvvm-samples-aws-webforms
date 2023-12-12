using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using Altairis.VtipBaze.Data;

namespace Altairis.VtipBaze.Import
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var dc = new VtipBazeContext())
            {
                dc.Database.CreateIfNotExists();

                // seed data (all jokes were kindly provided by ChatGPT)
                if (!dc.Tags.Any())
                {
                    dc.Tags.Add(new Tag()
                    {
                        TagName = "Puns",
                        Jokes = new List<Joke>()
                        {
                            new Joke() { Text = "I'm reading a book on the history of glue. I just can't seem to put it down.", Approved = true },
                            new Joke() { Text = "Why did the bicycle fall over? Because it was two-tired.", Approved = true },
                            new Joke() { Text = "I told my wife she was drawing her eyebrows too high. She looked surprised.", Approved = true },
                            new Joke() { Text = "Why don't scientists trust atoms? Because they make up everything.", Approved = true },
                            new Joke() { Text = "I used to play piano by ear, but now I use my hands.", Approved = true }
                        }
                    });
                    dc.Tags.Add(new Tag()
                    {
                        TagName = "KnockKnock",
                        Jokes = new List<Joke>()
                        {
                            new Joke() { Text = "Knock, knock.\r\nWho's there?\r\nBoo.\r\nBoo who?\r\nDon't cry. It's just a joke.", Approved = true },
                            new Joke() { Text = "Knock, knock.\r\nWho's there?\r\nNobel.\r\nNobel who?\r\nNo bell, that's why I knocked.", Approved = true },
                            new Joke() { Text = "Knock, knock.\r\nWho's there?\r\nOrange.\r\nOrange who?\r\nOrange you going to answer the door?", Approved = true },
                            new Joke() { Text = "Knock, knock.\r\nWho's there?\r\nTank.\r\nTank who?\r\nYou're welcome.", Approved = true },
                            new Joke() { Text = "Knock, knock.\r\nWho's there?\r\nAlpaca.\r\nAlpaca who?\r\nAlpaca the suitcase, you load up the car!", Approved = true },
                            new Joke() { Text = "Knock, knock.\r\nWho's there?\r\nLettuce.\r\nLettuce who?\r\nLettuce in, it's cold out here!", Approved = true },
                            new Joke() { Text = "Knock, knock.\r\nWho's there?\r\nInterrupting cow.\r\nInterrupting cow wh-\r\nMOOOOOOOOO!", Approved = true },
                            new Joke() { Text = "Knock, knock.\r\nWho's there?\r\nHarry.\r\nHarry who?\r\nHarry up and answer the door!", Approved = true }
                        }
                    });
                    dc.Tags.Add(new Tag()
                    {
                        TagName = "Animal",
                        Jokes = new List<Joke>()
                        {
                            new Joke() { Text = "Why did the chicken cross the playground? To get to the other slide.", Approved = true },
                            new Joke() { Text = "What do you call a bear with no teeth? A gummy bear.", Approved = true },
                            new Joke() { Text = "Why did the frog call his insurance company? He had a jump in his car.", Approved = true },
                            new Joke() { Text = "What do you call a sleeping bull? A bulldozer.", Approved = true },
                            new Joke() { Text = "What do you call a group of cows playing instruments? A moo-sical band.", Approved = true },
                            new Joke() { Text = "Why did the elephant wear green sneakers? Because the red ones were in the wash.", Approved = true },
                            new Joke() { Text = "What did the grape say when the elephant stepped on it? Nothing, it just let out a little wine.", Approved = true },
                            new Joke() { Text = "Why did the cat wear a fancy dress? She was feline fine.", Approved = true }
                        }
                    });
                    dc.Tags.Add(new Tag()
                    {
                        TagName = "Political",
                        Jokes = new List<Joke>()
                        {
                            new Joke() { Text = "Why did the politician decide to retire? He wanted to spend more time raising campaign funds.", Approved = true },
                            new Joke() { Text = "Why did the politician refuse to take a polygraph test? He didn't want to be hooked up to a machine that was smarter than he was.", Approved = true },
                            new Joke() { Text = "Why did the politician wear two jackets when he painted the house? The instructions on the can said: \"Put on two coats for best results.\"", Approved = true },
                            new Joke() { Text = "Why did the politician go to the doctor? He needed a prescription for his addiction to lobbyists' money.", Approved = true },
                        }
                    });
                    dc.SaveChanges();

                    Console.WriteLine("Data seed completed.");
                }
                else
                {
                    Console.WriteLine("The database already exists, skipped.");
                }

                var adminUsers = Membership.FindUsersByName("admin");
                if (adminUsers.Count == 0)
                {
                    var user = Membership.CreateUser("admin", "admin123", "test@mail.local");
                    user.IsApproved = true;
                    Membership.UpdateUser(user);

                    Console.WriteLine("Created admin user.");
                }
                else
                {
                    Console.WriteLine("Admin user already exists, skipped.");
                }
            }
        }

        
    }

}
