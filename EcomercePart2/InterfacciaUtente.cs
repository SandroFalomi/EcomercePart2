using EcomercePart2;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace EcomerceEs
{
    public static class InterfacciaUtente
    {
        private static ordersEntities ctx = new ordersEntities();
        public static bool VisualizzaMenu()
        {

            char sceltaUtente;

            do
            {
                Console.Clear();
                Console.WriteLine("Buongiorno, ha già un account?\n1)Accedi\n2)Registrati\n3)Exit");
                ConsoleKeyInfo po = Console.ReadKey();
                sceltaUtente = po.KeyChar;
            } while (sceltaUtente.Equals('1') == false && sceltaUtente.Equals('2') == false && sceltaUtente.Equals('3') == false);

            if(sceltaUtente == '3')
            {
                return false;
            }

            Console.Clear();

            bool risultato = true;

            do
            {
                string[] credentials = askCredentials().Split(';');
                if (sceltaUtente.Equals('1'))
                {


                    risultato = !Login(credentials[0], credentials[1]);
                }
                else
                {
                    risultato = !CreateNewAccount(credentials[0], credentials[1]);
                }
                Console.Clear();
                Console.WriteLine(risultato == false ? "Login avvenuto con successo" : "Qualcosa è andato storto");
                Console.WriteLine("\nPremere un tasto per andare avanti...");
                Console.ReadKey();
            } while (risultato);



            bool exit = true;

            do
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("Che cosa desideri fare?" +
                                        "\n1) Lista degli ordini" +
                                        "\n2) Dettaglio Ordine" +
                                        "\n3) Creazione di un nuovo ordine" +
                                        "\n4) Exit");
                    ConsoleKeyInfo po = Console.ReadKey();
                    sceltaUtente = po.KeyChar;
                } while (sceltaUtente.Equals('1') == false && sceltaUtente.Equals('2') == false && sceltaUtente.Equals('3') == false && sceltaUtente.Equals('4') == false);

                Console.Clear();

                switch (sceltaUtente)
                {
                    case '1':
                        Console.WriteLine("1) Lista degli ordini");
                        foreach(var item in ctx.orders)
                        {
                            Console.WriteLine($"\n{item.orderid}) {item.customer}\t {item.orderdate}\n");
                            foreach(var item2 in item.orderitems)
                            {
                                Console.WriteLine($"\t{item2.item} {item2.qty} {item2.price}");
                            }
                        }
                        Console.ReadKey();
                        break;
                    case '2':
                        bool esci = true;
                        string pippo = "";
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("2) Dettaglio Ordine");
                            Console.WriteLine("Inserire nome di chi si vuole visualizzare l'ordine");
                            string input = Console.ReadLine().ToLower();
                            int x = 0;
                            foreach (var item in ctx.orders)
                            {
                                if (item.customer.ToLower().Equals(input))
                                {
                                    Console.WriteLine($"\n{item.orderid}) {item.customer}\t {item.orderdate}\n");
                                    foreach (var item2 in item.orderitems)
                                    {
                                        Console.WriteLine($"\t{item2.item} {item2.qty} {item2.price}");
                                    }
                                    x++;
                                }
                            }
                            if(x != 0)
                            {
                                esci = false;
                                Console.ReadKey();
                            }
                            else
                            {
                                Console.WriteLine("La persona inserita non ha fatto acquisti con noi, prego premere un tasto per riprovare\nPremere E per uscire");
                                ConsoleKeyInfo y = Console.ReadKey();
                                pippo = ("" + y.KeyChar).ToLower();
                            }

                        } while (esci && pippo != "e");
                        break;
                    case '3':

                        string pippa = "";
                        List<orderitems> itemsdaaggiungere = new List<orderitems>();
                        int controllo = -1;
                        bool firsttime = true;

                        do
                        {
                            
                            Console.Clear();
                            Console.WriteLine("3) Creazione di un nuovo ordine");
                            Console.WriteLine("Inserire id dell'oggetto che si vuole acquistare\nPremere C per confermare");
                            if(firsttime == false)
                            {
                                if (controllo < -1 || controllo > ctx.items.Count())
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\nItem inesistenete, premere qualunque tasto per riprovare\n");
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("\nItem aggiunto al carrello\n");
                                    Console.ResetColor();
                                    itemsdaaggiungere.Add(ctx.orderitems.Find(pippa));
                                }
                            }
                            else
                            {
                                Console.WriteLine("\n\n\n");
                                firsttime = false;
                            }
                            
                            controllo = -1;
                            int i = 0;
                            foreach (var itema in ctx.orderitems)
                            {
                                i++;
                                Console.WriteLine($"{i}) {itema.item}\n");
                            }
                            pippa = Console.ReadLine();
                            int.TryParse( pippa, out controllo );
                            
                        } while (pippa.ToLower() != "c");

                        firsttime = true;

                        do
                        {

                            Console.Clear();
                            Console.WriteLine("3) Creazione di un nuovo ordine");
                            Console.WriteLine("Scegliere il cliente che ha effetuato l'ordine");
                            if (firsttime == false)
                            {
                                if (controllo < -1 || controllo > ctx.customers.Count())
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\nCustomer inesistenete, riprovare\n");
                                    Console.ResetColor();
                                }
                            }
                            else
                            {
                                firsttime = false;
                            }

                            int i = 0;

                            foreach (var itema in ctx.customers)
                            {
                                i++;
                                Console.WriteLine($"{i}) {itema.customer}\n");
                            }

                            pippa = Console.ReadLine();
                            int.TryParse(pippa, out controllo);

                        } while (controllo < -1 || controllo > ctx.customers.Count());

                        List<orderitems> daAggiungere = new List<orderitems>();

                        foreach(var itema in itemsdaaggiungere)
                        {
                            var ciao = daAggiungere.Where(sidy => sidy.item == itema.item).First();
                            if (ciao == null)
                            {
                                itema.qty = 1;
                                daAggiungere.Add(itema);
                            }
                            else
                            {
                                daAggiungere.Where(sidy => sidy.item == itema.item).First().qty = ciao.qty + 1;
                            }
                        }

                        foreach(var itema in ctx.orders)
                        {
                            if(itema.customer == ctx.customers.Find(controllo).customer)
                            {
                                itema.orderitems
                            }
                        }

                        

                        

                        break;
                    case '4':
                        exit = false;
                        break;
                }


                
            } while (exit);
            Console.Clear();
            return true;
        }

        private static string askCredentials()
        {
            Console.Clear();
            Console.WriteLine("Inserire User: ");
            string user = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Inserire Password: ");
            string password = Console.ReadLine();
            return user + ";" + password;
        }

        private static bool Login(string user, string password)
        {
            foreach(var item in ctx.Utenti)
            {
                if (item.login == user && item.password == password)
                    return true;
            }
            return false;
        }
        private static bool CreateNewAccount(string user, string password)
        {
            foreach (var item in ctx.Utenti)
            {
                if (item.login == user)
                    return false;
            }

            ctx.Utenti.Add(new Utenti() { login = user, password = password });

            return true;

        }

    }

    
}

