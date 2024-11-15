using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
{
    internal class Program
    {
        static List<Member> members = new List<Member>();
        static List<Book> books = new List<Book>();
        static List<Issued> issues = new List<Issued>();

        static string getDateTime()
        {
            return DateTime.Now.ToString();
        }

        static string path = "action_logs.txt";
        static string reportPath = "report.csv";

        static void Main(string[] args)
        {
            bool loopController = true; // used to handlle the while loop

            if (!File.Exists(path))
            {
                File.Create(path);
            }

            while (loopController)
            {
                Console.WriteLine("|----- Select a operator -----|\n");
                Console.WriteLine("1 - Add Book");
                Console.WriteLine("2 - Add Member");
                Console.WriteLine("3 - Issue Book");
                Console.WriteLine("4 - Return Book");
                Console.WriteLine("5 - View Issued Books");
                Console.WriteLine("6 - Exit\n");

                Console.Write("Enter the operator :");
                int Operator = Convert.ToInt32(Console.ReadLine());

                switch (Operator)
                {
                    case 1:
                        AddBook();
                        break;
                    case 2:
                        AddMember();
                        break;
                    case 3:
                        IssueBooK();
                        break;
                    case 4:
                        ReturnBook();
                        break;
                    case 5:
                        ViewIssuedBooks();
                        break;
                    default:
                        loopController = false;
                        break;
                }
            }

        }

        static void handleReport() // used to generate the CSV file
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(Program.reportPath))
                {
                    sw.WriteLine("ISBN, NIC, Issued_Date, Deadline");
                    foreach (var issue in Program.issues)
                    {
                        sw.WriteLine($"{issue.ISBN},\"{issue.NIC}\",{issue.IssuedDate},{issue.Deadline}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be written:");
                Console.WriteLine(e.Message);
            }
        }

        static void AddMember()
        {
            Console.Write("Enter the NIC number :");
            string NIC = Console.ReadLine();

            bool isUserExist = false;
            foreach (var member in Program.members) //used to prevent creating same member
            {
                if (member.NIC == NIC)
                {
                    isUserExist = true;
                    Console.WriteLine("This member is already added.");
                    break;
                }
            }

            if (!isUserExist) //if user is not registed, Now continue...
            {
                Console.Write("Enter the name :");
                string name = Console.ReadLine();

                Console.Write("Enter the phone number :");
                string mobile = Console.ReadLine();

                Console.Write("Enter the address :");
                string address = Console.ReadLine();

                Program.members.Add(new Member(NIC, name, mobile, address));
                Console.WriteLine("New member has been added successfully.");
                File.AppendAllText(Program.path, $"{NIC} - Member has been added successfully at {getDateTime()}\n");
            }



        }

          }
    }
}
