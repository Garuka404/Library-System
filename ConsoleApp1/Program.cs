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
        static void AddBook()
        {
            Console.Write("Enter the ISBN :");
            string ISBN = Console.ReadLine();

            bool checkISBN = true;

            foreach (var book in Program.books) //used to prevent ISBN number conflit
            {
                if (ISBN == book.ISBN)
                {
                    checkISBN = false;
                    Console.WriteLine("ISBN number is already used in the system.");
                    break;
                }
            }

            if (checkISBN) //if ISBN number OK, continue...
            {
                Console.Write("Enter the title :");
                string title = Console.ReadLine();

                Console.Write("Enter the author :");
                string author = Console.ReadLine();

                Program.books.Add(new Book(title, author, ISBN));
                Console.WriteLine("New book has been added successfully.");
                File.AppendAllText(Program.path, $"{ISBN} - New book has been added successfully at {getDateTime()}\n");
            }


        }
        static void IssueBooK()
        {
            Console.Write("Enter the ISBN :");
            string ISBN = Console.ReadLine();

            bool ckeckRequest = false;
            bool availablity = true;

            foreach (var book in Program.books) // find ISBN number valid or not
            {
                if (book.ISBN == ISBN)
                {
                    ckeckRequest = true;
                    break;
                }
            }

            if (!ckeckRequest) // this notify that ISBN is not valid
            {
                Console.WriteLine("Bad request, please check ISBN");
            }

            else
            {
                foreach (var issue in Program.issues) // check that book is issued or not using issued-Book-List
                {
                    if (issue.ISBN == ISBN)
                    {
                        Console.WriteLine("This book is not available now");
                        availablity = false;
                    }
                }

                if (availablity) // if is not issued, go to the next step
                {
                    bool isMember = false;

                    Console.Write("Enter the NIC number :");
                    string NIC = Console.ReadLine();

                    foreach (var item in Program.members) // check member is registered or not using member-List
                    {
                        if (item.NIC == NIC)
                        {
                            isMember = true;
                            break;
                        }
                    }

                    if (isMember) // if user is registed as member, now continue...
                    {
                        string[] dateAndTime = getDateTime().ToString().Split();
                        string date = dateAndTime[0];

                        DateTime newDate = DateTime.Now.AddDays(14);
                        string[] deadlineDateAndTime = newDate.ToString().Split();
                        string deadline = deadlineDateAndTime[0];

                        Program.issues.Add(new Issued(ISBN, NIC, date, deadline));
                        Program.handleReport();
                        Console.WriteLine("Book has been issued successfully.");
                        File.AppendAllText(Program.path, $"{ISBN} - Book has been issued to {NIC} successfully at {getDateTime()}\n");
                    }

                    else
                    {
                        Console.WriteLine("This user isn't registered as a member"); // if user is not registed, not allowed to do that
                    }
                }

                else
                {
                    Console.WriteLine("Book is not available now"); // if book is already issued, display this error message
                }
            }

        }


    }

}
