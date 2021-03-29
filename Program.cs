using FinchAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Control
    // Application Type: Console
    // Description: The debute of the incredable cornbread and his data colection
    // Author: Severance, Scott
    // Dated Created: 2/17/2021
    // Last Modified: 2/28/2021
    //
    // **************************************************

    enum Command
    {
        movefoward,
        movebackward,
        turnleft,
        turnright,
        stop,
        wait,
        ledon,
        ledoff,
        gettempature,
        react,
        done,
    }
    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            SetTheme();
            DisplayWelcomeScreen();
            string dummyUser = LoginCheck();
            DisplayMenuScreen(dummyUser);
            DisplayClosingScreen();
        }

        static string LoginCheck()
        {
            string dummyUser = null;
            try
            {
                Console.CursorVisible = true;
                string dataPath = @"C:\Finch-Control-master\Project_FinchControl\Data\User_Info.txt";
                string[] Info = File.ReadAllLines(dataPath);
                List<string> Users = new List<string>();
                List<string> Password = new List<string>();
                if (Info.Length == 4)
                {
                    foreach (string account in Info)
                    {
                        string[] entries = account.Split(',');
                        Users.Add(entries[0]);
                        Password.Add(entries[1]);
                    }
                    bool loginEntry = false;
                    do
                    {
                        DisplayScreenHeader("Login");
                        Console.WriteLine();
                        Console.WriteLine("\tPlease login with your username and password so data can be saved");
                        Console.WriteLine("\tOr sign in with the username Guest to continue without saving");
                        Console.WriteLine();
                        Console.WriteLine("Username:");
                        dummyUser = Console.ReadLine();
                        if(dummyUser == "Guest")
                        {
                            loginEntry = true;
                        }
                        else if (Users.Contains(dummyUser))
                        {
                            Console.WriteLine("Password:");
                            string dummyPass = Console.ReadLine();
                            if (Password[Users.IndexOf(dummyUser)] == dummyPass)
                            {
                                loginEntry = true;
                            }
                            else
                            {
                                Console.WriteLine("Password is incorrect");
                                Console.WriteLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Username is not found");
                        }
                        DisplayContinuePrompt();
                    } while (!loginEntry);
                }
                else if (Info.Length < 4)
                {
                    Console.WriteLine("\tThe File User_Info.txt is missing information");
                    Console.WriteLine("Please Redownload User_Info.txt and restart the application");
                    DisplayContinuePrompt();
                    DisplayClosingScreen();
                    System.Environment.Exit(0);
                }
                else if (Info.Length > 4)
                {
                    Console.WriteLine("\tThe File User_Info.txt has excess information");
                    Console.WriteLine("Please Redownload User_Info.txt and restart the application");
                    DisplayContinuePrompt();
                    DisplayClosingScreen();
                    System.Environment.Exit(0);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("\tThe File User_Info.txt is missing or missplaced");
                Console.WriteLine("Please Redownload User_Info.txt and restart the application");
                DisplayContinuePrompt();
                DisplayClosingScreen();
                System.Environment.Exit(0);
            }
            return dummyUser;
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Main Menu                                 *
        /// *****************************************************************
        /// </summary>
        static void DisplayMenuScreen(string dummyUser)
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;
            bool robotConnected = false;

            Finch cornBread = new Finch();

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine("\te) User Programming");
                Console.WriteLine("\tf) Disconnect Finch Robot");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        robotConnected = DisplayConnectFinchRobot(cornBread);
                        break;

                    case "b":
                        TestingForConnection(robotConnected);
                        if (robotConnected == true)
                        {
                            TalentShowDisplayMenuScreen(cornBread);
                        }

                        break;

                    case "c":
                        TestingForConnection(robotConnected);
                        if (robotConnected == true)
                        {
                            DataRecorderDisplayMenuScreen(cornBread, dummyUser);
                        }
                        break;


                    case "d":
                        TestingForConnection(robotConnected);
                        if (robotConnected == true)
                        {
                            AlarmSystemDisplayMenuScreen(cornBread);
                        }
                        break;

                    case "e":
                        TestingForConnection(robotConnected);
                        if (robotConnected == true)
                        {
                            UserProgrammingDisplayMenuScreen(cornBread, dummyUser);
                        }
                        break;

                    case "f":
                        DisplayDisconnectFinchRobot(cornBread);
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(cornBread);
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        #region USER PROGRAMMING
        /// <summary>
        /// 
        ///     User Programming menu option for finch robot
        /// 
        /// </summary>
        /// <param name="cornBread"></param>
        static void UserProgrammingDisplayMenuScreen(Finch cornBread, string dummyUser)
        {
            Console.CursorVisible = true;

            bool quitUserProgrammingMenu = false;
            string menuChoice;

            List<(Command, int, double)> program = new List<(Command, int, double)>();

            do
            {
                DisplayScreenHeader("User Programming");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Add Commands");
                Console.WriteLine("\tb) View Commands");
                Console.WriteLine("\tc) Run Commands");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");

                Console.WriteLine();
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        (program) = UserProgrammingDisplayAddCommand();
                        break;

                    case "b":
                        UserProgrammingDisplayViewCommand(program);
                        break;

                    case "c":
                        UserProgrammingDisplayRunCommand(program, cornBread);
                        break;

                    case "q":
                        quitUserProgrammingMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitUserProgrammingMenu);
        }

        #region ADD COMMAND
        static List<(Command, int, double)> UserProgrammingDisplayAddCommand()
        {
            List<(Command, int, double)> program = new List<(Command, int, double)>();
            bool Done = false;
            int num = 1;

            UserProgrammingDisplayAddCommandStartup();
            do
            {
                string dummy = "null";
                int value = 0;
                double time = 0;
                string reaction = "null";

                Console.Clear();
                UserProgrammingDisplayCommandOptions();
                Console.WriteLine();
                UserProgrammingDisplayCommands(program);
                Console.WriteLine();
                Console.WriteLine($"Line {num}:     ");
                dummy = Console.ReadLine().ToLower();
                if (Enum.TryParse<Command>(dummy, out Command EnumDummy))
                {
                    num = num + 1;
                    if (dummy == "movefoward" || dummy == "movebackward" || dummy == "turnleft" || dummy == "turnright"|| dummy == "ledon")
                    {
                        // Branch for seperate catagory
                        switch (dummy)
                            {
                        case "ledon":
                            Console.Write("LED Light Level: {1-255}    ");
                                value = UserProgrammingGetValue();
                                break;

                        default:
                            Console.Write("Motor Power: {1-255}    ");
                                value = UserProgrammingGetValue();
                                time = UserProgrammingGetTime();
                                break;
                        }
                        program.Add((EnumDummy, value, time));
                    }
                    else if (dummy == "react")
                    {
                        bool React = false;
                        do
                        {
                            Console.Write("Reaction: {Happy, Sad, Mad}    ");
                            reaction = Console.ReadLine().ToLower();
                            if (reaction == "happy" || reaction == "sad" || reaction == "mad")
                            {
                                switch (reaction)
                                {
                                    case "happy":
                                        program.Add((EnumDummy, 1, 0));
                                        break;
                                    case "sad":
                                        program.Add((EnumDummy, 2, 0));
                                        break;
                                    case "mad":
                                        program.Add((EnumDummy, 3, 0));
                                        break;
                                }
                                React = true;
                            }
                            else
                            {
                                Console.WriteLine("Unknown Command: Make sure you are using commands from the list above");
                            }
                        } while (!React);
                    }
                    else if (dummy == "wait")
                    {
                        time = UserProgrammingGetTime();
                        program.Add((EnumDummy, 0, time));
                    }
                    else if (dummy == "done")
                    {
                        Done = true;
                    }
                    else
                    {
                        program.Add((EnumDummy, 0, 0));
                    }
                    dummy = "null";
                }
                else
                {
                    Console.WriteLine("Unknown Command: Make sure you are using commands from the list above");
                    DisplayContinuePrompt();
                }
                
            } while (!Done);

            DisplayContinuePrompt();
            return program;
        }

        static int UserProgrammingGetValue()
        {
            bool Value = false;
            int value;
            do
            {
                if (int.TryParse(Console.ReadLine(), out value))
                {
                    if (value > 0 && value < 255)
                    {
                    Value = true;
                    }
                    else
                    {
                    Console.WriteLine("The entered value must be within the limits");
                    }
                }
                else
                {
                    Console.WriteLine("The entered value must be a number");
                }

            } while (!Value);
            return value;
        }

        static double UserProgrammingGetTime()
        {
            bool Time = false;
            double time;
            do
            {
                Console.Write("How long in seconds:      ");
                if (double.TryParse(Console.ReadLine(), out time))
                {
                    Time = true;
                }
                else
                {
                    Console.WriteLine("The entered value must be a number");
                }

            } while (!Time);
            return time;
        }

        static void UserProgrammingDisplayAddCommandStartup()
        {
            DisplayScreenHeader("Add Command");
            Console.WriteLine("\tPlease type the commands you wish to perform in the order you wish to perform them");
            Console.WriteLine();
            DisplayContinuePrompt();
        }

        static void UserProgrammingDisplayCommandOptions()
        {
            Console.WriteLine("\tOptions:");
            foreach (string command in Enum.GetNames(typeof(Command)))
            {
                Console.Write(command + " | ");
            }
           Console.WriteLine();
        }
        #endregion

        #region VIEW COMMAND
        static void UserProgrammingDisplayViewCommand(List<(Command, int, double)> program)
        {
            DisplayScreenHeader("View Command");
            UserProgrammingDisplayCommands(program);
            DisplayContinuePrompt();
        }
        #endregion

        #region RUN COMMAND
        private static void UserProgrammingDisplayRunCommand(List<(Command, int, double)> program, Finch cornBread)
        {
            DisplayScreenHeader("Run Command");
            UserProgrammingRunCommand(program, cornBread);
            DisplayContinuePrompt();
        }

        static void UserProgrammingRunCommand(List<(Command, int, double)> program, Finch cornbread)
        {
            int displayNum = 1;
            foreach ((Command, int, double) command in program)
            {
                UserProgrammingDisplayCommandIndividual(command, displayNum);
                displayNum += 1;

                switch (command.Item1)
                {
                    case Command.movefoward:
                        cornbread.setMotors(command.Item2, command.Item2);
                        cornbread.wait((int)((command.Item3) * 1000));
                        break;

                    case Command.movebackward:
                        cornbread.setMotors(-command.Item2, -command.Item2);
                        cornbread.wait((int)((command.Item3) * 1000));
                        break;

                    case Command.turnleft:
                        cornbread.setMotors(command.Item2, -command.Item2);
                        cornbread.wait((int)((command.Item3) * 1000));
                        break;

                    case Command.turnright:
                        cornbread.setMotors(-command.Item2, command.Item2);
                        cornbread.wait((int)((command.Item3) * 1000));
                        break;

                    case Command.stop:
                        cornbread.setMotors(0, 0);
                        break;

                    case Command.wait:
                        cornbread.wait((int)((command.Item3) * 1000));
                        break;

                    case Command.ledon:
                        cornbread.setLED(command.Item2, command.Item2, command.Item2);
                        break;
                    case Command.ledoff:
                        cornbread.setLED(0, 0, 0);
                        break;
                    case Command.gettempature:
                        Console.WriteLine($"Tempature is {cornbread.getTemperature()} Celcius");
                        break;
                    case Command.react:
                        switch (command.Item2)
                        {
                            case 1:
                                cornbread.noteOn(261);
                                cornbread.wait(300);
                                cornbread.noteOn(293);
                                cornbread.wait(100);
                                cornbread.noteOn(350);
                                cornbread.wait(400);
                                cornbread.noteOff();
                                break;
                            case 2:
                                cornbread.noteOn(247);
                                cornbread.wait(200);
                                cornbread.noteOn(196);
                                cornbread.wait(100);
                                cornbread.noteOn(165);
                                cornbread.wait(300);
                                cornbread.noteOff();
                                break;
                            case 3:
                                cornbread.noteOn(175);
                                cornbread.wait(500);
                                cornbread.noteOn(165);
                                cornbread.wait(400);
                                cornbread.noteOff();
                                break;
                        }
                        break;
                }
            }
            cornbread.setMotors(0, 0);
            cornbread.setLED(0, 0, 0);
            cornbread.noteOff();
        }

        #endregion
        static void UserProgrammingDisplayCommands(List<(Command,int,double)> program)
        {
            int displayNum = 1;
            Console.WriteLine($"        Command".PadLeft(10) + $"Value".PadLeft(20) + $"Time".PadLeft(10));
            Console.WriteLine($"        ~~~~~~~".PadLeft(10) + $"~~~~~".PadLeft(20) + $"~~~~".PadLeft(10));
            foreach ((Command, int, double) command in program)
            {
                UserProgrammingDisplayCommandIndividual(command, displayNum);
                displayNum += 1;
            }

        }
        static void UserProgrammingDisplayCommandIndividual((Command, int, double) command, int displayNum)
        {
            string reaction = "null";
            if (command.Item1 == Command.react)
            {
                switch (command.Item2)
                {
                    case 1:
                        reaction = "happy";
                        break;
                    case 2:
                        reaction = "sad";
                        break;
                    case 3:
                        reaction = "mad";
                        break;
                }

                Console.WriteLine($"Line {displayNum}: {command.Item1}".PadLeft(10) + $"{reaction}".PadLeft(20) + $"NA".PadLeft(10));
            }
            else if (command.Item1 == Command.gettempature || command.Item1 == Command.ledoff || command.Item1 == Command.stop)
            {
                Console.WriteLine($"Line {displayNum}: {command.Item1}".PadLeft(10) + $"NA".PadLeft(20) + $"NA".PadLeft(10));
            }
            else if (command.Item1 == Command.ledon)
            {
                Console.WriteLine($"Line {displayNum}: {command.Item1}".PadLeft(10) + $"{command.Item2}".PadLeft(20) + $"NA".PadLeft(10));
            }
            else if (command.Item1 == Command.wait)
            {
                Console.WriteLine($"Line {displayNum}: {command.Item1}".PadLeft(10) + $"NA".PadLeft(20) + $"{command.Item3}".PadLeft(10));
            }
            else
            {
                Console.WriteLine($"Line {displayNum}: {command.Item1}".PadLeft(10) + $"{command.Item2}".PadLeft(20) + $"{command.Item3}".PadLeft(10));
            }
        }
        #endregion

        #region ALARM SYSTEM

        //~~~~~~~~~~~~~~~~~~~~~~~~
        //
        //  Alarm System Menu
        //
        //~~~~~~~~~~~~~~~~~~~~~~~~
        #region ALARM SYSTEM MENU

        /// <summary>
        /// 
        ///     Alarm system menu option for finch robot
        /// 
        /// </summary>
        /// <param name="cornBread"></param>
        static void AlarmSystemDisplayMenuScreen(Finch cornBread)
        {
            Console.CursorVisible = true;

            bool quitTalentShowMenu = false;
            string menuChoice;

            //
            //Information Values
            //
            string dataType = "null";
            string sensorSelection = "null";
            string[] rangeType = new string[2];
            int timeToMonitor = 0;
            int[] minMaxThreashold = new int[2];

            //
            //Menu Test Values
            //
            (string, string, string, string, string) testSet = ("0", "0", "0", "0", "0");
            var (setSensor, setRange, minMax, setTime, alarmLive) = testSet;
            int dummy = 0;

            do
            {
                DisplayScreenHeader("Alarm System");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Sensors to Monitor");
                Console.WriteLine("\tb) Set Range Type");
                Console.WriteLine("\tc) Set Min/Max Threashold");
                Console.WriteLine("\td) Set Time to Monitor");
                Console.WriteLine("\te) Set Alarm");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");

                Console.WriteLine();
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        dataType = AlarmSystemDisplaySetDataType();

                        if (dataType == "tempature")
                        {
                            DisplayMenuPrompt("Alarm System");
                        }
                        else
                        {
                            //
                            //Extra Step for which Light Sensor
                            //
                            sensorSelection = AlarmSystemDisplaySetSensor();
                        }
                        setSensor = "done";
                        //
                        //Reset Values to remove temp to light confusion
                        //
                        setRange = "null";
                        minMax = "null";
                        break;

                    case "b":
                        setRange = "live";
                        dummy = AlarmSystemMenuTest(setSensor, setRange, minMax, setTime, alarmLive);
                        setRange = "0";
                        //
                        //If Requirments are met
                        //
                        if (dummy == 1)
                        {
                            rangeType = AlarmSystemDisplayGetRangeType(dataType);
                            setRange = "done";
                        }
                        break;

                    case "c":
                        minMax = "live";
                        dummy = AlarmSystemMenuTest(setSensor, setRange, minMax, setTime, alarmLive);
                        minMax = "0";
                        //
                        //If Requirments are met
                        //
                        if (dummy == 1)
                        {
                            minMaxThreashold = AlarmSystemDisplayGetMinMaxThreashold(sensorSelection, rangeType, cornBread, dataType);
                            minMax = "done";
                        }
                        dummy = 0;
                        break;

                    case "d":
                        timeToMonitor = AlarmSystemDisplaySetTime();
                        setTime = "done";
                        break;

                    case "e":
                        alarmLive = "live";
                        dummy = AlarmSystemMenuTest(setSensor, setRange, minMax, setTime, alarmLive);
                        alarmLive = "0";
                        //
                        //If Requirments are met
                        //
                        if (dummy == 1)
                        {
                            AlarmSystemDisplayAlarmSet(sensorSelection, rangeType, minMaxThreashold, timeToMonitor, cornBread, dataType);
                        }
                        dummy = 0;
                        break;

                    case "q":
                        quitTalentShowMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitTalentShowMenu);
        }

        static int AlarmSystemMenuTest(string setSensor, string setRange, string minMax, string setTime, string alarmLive)
        {

            int dummy = 0;
            if (setRange == "live" && setSensor != "done")
            {
                DisplayScreenHeader("Set Range Type");
                Console.WriteLine();
                Console.WriteLine("\tUnable to record data");
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\tMissing information in menu options: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\tSet Sensors to Monitor");
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Black;
                DisplayMenuPrompt("Alarm System");
            }
            else if (minMax == "live" && (setRange != "done" || setSensor != "done"))
            {
                DisplayScreenHeader("Set Min/Max Threashold");
                Console.WriteLine();
                Console.WriteLine("\tUnable to record data");
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\tMissing information in menu options: ");
                Console.ForegroundColor = ConsoleColor.White;
                if (setSensor != "done")
                {
                    Console.WriteLine("\tSet Sensors to Monitor");
                }
                if (setRange != "done")
                {
                    Console.WriteLine("\tSet Range Type ");
                }
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Black;
                DisplayMenuPrompt("Alarm System");
            }
            else if (alarmLive == "live" && (setSensor != "done" || setRange != "done" || minMax != "done" || setTime != "done"))
            {
                DisplayScreenHeader("Alarm System");
                Console.WriteLine();
                Console.WriteLine("\tUnable to record data");
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\tMissing information in menu options: ");
                Console.ForegroundColor = ConsoleColor.White;

                if (setSensor != "done")
                {
                    Console.WriteLine("\tSet Sensors to Monitor");
                }
                if (setRange != "done")
                {
                    Console.WriteLine("\tSet Range Type ");
                }
                if (minMax != "done")
                {
                    Console.WriteLine("\tSet Min/Max Threashold");
                }
                if (setTime != "done")
                {
                    Console.WriteLine("\tSet Time to Monitor");
                }
                Console.WriteLine("");

                Console.ForegroundColor = ConsoleColor.Black;
                DisplayMenuPrompt("Alarm System");
            }
            else
            {
                dummy = 1;
            }

            return dummy;
        }

        #endregion

        //~~~~~~~~~~~~~~~~~~~~~~~~
        //
        //  Alarm System Options
        //
        //~~~~~~~~~~~~~~~~~~~~~~~~
        #region ALARM SYSTEM OPTIONS

        #region ALARM SYSTEM DATA TYPE/SENSOR TYPE

        /// <summary>
        /// Menu Option for Set Sensor
        /// </summary>
        /// <returns></returns>
        static string AlarmSystemDisplaySetDataType()
        {
            bool Type;
            string dataType = "null";
            do
            {
                Type = true;

                DisplayScreenHeader("Set Sensors to Monitor");
                Console.WriteLine("\tEnter what data to record: [tempature, light, both]");
                Console.WriteLine();

                dataType = Console.ReadLine().ToLower();
                if (dataType != "tempature" && dataType != "light" && dataType != "both")
                {
                    Type = false;
                    Console.WriteLine("\tUnknown Selection");
                    Console.WriteLine("Ensure you are using one of the displayed options");
                    DisplayContinuePrompt();
                }
                else
                {
                    Console.WriteLine($"\t\tYou selected data type: {dataType}.");
                }


            } while (!Type);

            return dataType;
        }

        /// <summary>
        /// Menu Option for Set Sensor
        /// </summary>
        /// <returns></returns>
        static string AlarmSystemDisplaySetSensor()
        {
            bool Sensor;
            string setSensors = "null";

            //Checking What Light Sensor User Wants
            do
            {
                Sensor = true;
                Console.WriteLine();
                Console.WriteLine("\tEnter which light sensor to monitor: [left, right, both]");
                Console.WriteLine();

                setSensors = Console.ReadLine().ToLower();
                if (setSensors != "left" && setSensors != "right" && setSensors != "both")
                {
                    Sensor = false;
                    Console.WriteLine("\tUnknown Selection");
                    Console.WriteLine("Ensure you are using one of the displayed options");
                    DisplayContinuePrompt();
                }
                else
                {
                    Console.WriteLine($"\t\tYou selected sensor: {setSensors}.");
                }
            }
            while (!Sensor);

            DisplayMenuPrompt("Alarm System");
            return setSensors;
        }

        #endregion

        #region ALARM SYSTEM RANGE TYPE
        /// <summary>
        /// Menu Option for Get Range
        /// </summary>
        /// <returns></returns>
        static string[] AlarmSystemDisplayGetRangeType(string dataType)
        {
            string[] rangeType = new string[2];
            if (dataType == "both")
            {
                DisplayScreenHeader("Set Range Type");
                Console.WriteLine("You've decided to messure both tempature and light");
                Console.WriteLine("Starting with Tempature");
                Console.WriteLine();
                DisplayContinuePrompt();
                rangeType[0] = AlarmSystemRangeTest();
                rangeType[1] = AlarmSystemRangeTest();

            }
            else
            {
                rangeType[0] = AlarmSystemRangeTest();
            }

            DisplayMenuPrompt("Alarm System");
            return rangeType;
        }

        static string AlarmSystemRangeTest()
        {
            bool Range = true;
            string rangeType;
            do
            {
                DisplayScreenHeader("Set Range Type");
                Console.WriteLine("\tEnter the range to monitor: [maximum, minimum]");
                Console.WriteLine();
                rangeType = Console.ReadLine().ToLower();
                if (rangeType != "minimum" && rangeType != "maximum")
                {
                    Range = false;
                    Console.WriteLine("\tUnknown Selection");
                    Console.WriteLine("Ensure you are using one of the displayed options");
                    DisplayContinuePrompt();
                }
                else
                {
                    Console.WriteLine($"\t\tYou selected range: {rangeType}.");
                }
            } while (!Range);
            return rangeType;
        }
        #endregion

        #region ALARM STSYEM MIN/MAX

        /// <summary>
        /// Menu Option for Min/Max Threashold
        /// </summary>
        /// <returns></returns>
        static int[] AlarmSystemDisplayGetMinMaxThreashold(string sensorSelection, string[] rangeType, Finch cornbread, string dataType)
        {
            int dataL;
            int dataT;
            int[] minMaxThreashold = new int[2];
            bool MinMax;

            DisplayScreenHeader("Set Min/Max Threashold");
            Console.WriteLine("");
            //Getting Data based on Type and Sensor
            do
            {
                MinMax = false;
                //Getting Data
                (dataL, dataT) = AlarmSystemDisplayGetMinMaxValues(sensorSelection, dataType, cornbread);

                //Getting Data from User
                (minMaxThreashold, MinMax) = AlarmSystemMinMaxBroadValueTest(rangeType, dataL, dataType, dataT);
            } while (!MinMax);

            //Echoing User Data
            Console.WriteLine();
            if (dataType == "both")
            {
                Console.WriteLine($"\tYou selected to detect the light {rangeType[1]} of {sensorSelection} Sensor(s).");
                Console.WriteLine($"\tAnd detect the tempature {rangeType[0]}.");
                Console.WriteLine($"\t The tempature value you selected was {minMaxThreashold[0]}.");
                Console.WriteLine($"\t And the light value you selected was {minMaxThreashold[1]}.");
            }
            else if (dataType == "light")
            {
                Console.WriteLine($"\tYou selected to detect the light {rangeType[0]} of {sensorSelection} Sensor(s).");
                Console.WriteLine($"\t The value you selected was {minMaxThreashold[0]}.");
            }
            else
            {
                Console.WriteLine($"\tYou selected to detect the tempature {rangeType[0]}.");
                Console.WriteLine($"\t The value you selected was {minMaxThreashold[0]}.");
            }
            DisplayMenuPrompt("Alarm System");
            return minMaxThreashold;
        }

        /// <summary>
        /// Gets Data from the Finch
        /// </summary>
        /// <param name="sensorSelection"></param>
        /// <param name="dataType"></param>
        /// <param name="cornbread"></param>
        /// <returns></returns>
        static (int, int) AlarmSystemDisplayGetMinMaxValues(string sensorSelection, string dataType, Finch cornbread)
        {
            int dataL = 0;
            int dataT = 0;

            dataT = (int)cornbread.getTemperature();
            switch (sensorSelection)
            {
                case "right":
                    dataL = cornbread.getRightLightSensor();
                    break;

                case "left":
                    dataL = cornbread.getLeftLightSensor();
                    break;

                case "both":
                    int[] light = cornbread.getLightSensors();
                    dataL = (int)light.Average();
                    break;
            }
            return (dataL, dataT);
        }

        /// <summary>
        /// Organizes Data for Min Max Threashold
        /// </summary>
        /// <param name="rangeType"></param>
        /// <param name="data"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        static (int[], bool) AlarmSystemMinMaxBroadValueTest(string[] rangeType, int dataL, string dataType, int dataT)
        {
            string rangeDisplay = rangeType[0];
            int[] minMaxThreashold = new int[2];
            bool MinMax = false;

                DisplayScreenHeader("Set Min/Max Threashold");
            if (dataType == "both")
            {
                Console.WriteLine("\tYou've selected to monitor both tempature and light");
                Console.WriteLine("\tYou will need to set a threshold for both.");
                DisplayContinuePrompt();

                do
                {
                    DisplayScreenHeader("Set Min/Max Threashold");
                    Console.WriteLine($"Current Tempature Value: {dataT}");
                    int data = dataT;
                    (minMaxThreashold[0], MinMax) = AlarmSystemMinMaxSpecificValueTest(rangeDisplay, data, dataType);
                } while (!MinMax);

                do
                {
                    rangeDisplay = rangeType[1];
                    Console.WriteLine($"Current Light Value: {dataL}");
                    int data = dataL;
                    (minMaxThreashold[1], MinMax) = AlarmSystemMinMaxSpecificValueTest(rangeDisplay, data, dataType);
                } while (!MinMax);
            }
            else if (dataType == "light")
            {
                Console.WriteLine($"Current Light Value: {dataL}");
                int data = dataL;
                (minMaxThreashold[0], MinMax) = AlarmSystemMinMaxSpecificValueTest(rangeDisplay, data, dataType);
            }
            else
            {
                Console.WriteLine($"Current Tempature Value: {dataT}");
                int data = dataT;
                (minMaxThreashold[0], MinMax) = AlarmSystemMinMaxSpecificValueTest(rangeDisplay, data, dataType);
            }
                
            return (minMaxThreashold, MinMax);
        }

        /// <summary>
        /// Recives Data for the Mix Max Threashold
        /// </summary>
        /// <param name="rangeType"></param>
        /// <param name="data"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        static (int, bool) AlarmSystemMinMaxSpecificValueTest(string rangeType, int data, string dataType)
        {
            bool MinMax = false;
            int minMaxThreashold;
            Console.WriteLine();
            Console.WriteLine($"\tWhat number would you like to act as the {rangeType}?");
                string dummy = Console.ReadLine();
                //Checking User Submition
                if (int.TryParse(dummy, out minMaxThreashold))
                {
                    //If above Zero
                    if (minMaxThreashold < 0)
                    {
                        Console.WriteLine($"You can only detect {dataType} levels above or equal to 0");
                        DisplayContinuePrompt();
                    }
                    //If within range peramiters
                    else if ((rangeType == "maximum" && data < minMaxThreashold) || (rangeType == "minimum" && data > minMaxThreashold))
                    {
                        MinMax = true;
                    }
                    //If not winthin range peramiters
                    else if (rangeType == "maximum")
                    {
                        Console.WriteLine("Since you're testing for a maximum, ");
                        Console.WriteLine("you need to choose a value higher than the current level.");
                        DisplayContinuePrompt();
                    }
                    //If not winthin range peramiters
                    else
                    {
                        Console.WriteLine("Since you're testing for a minimum, ");
                        Console.WriteLine("you need to choose a value lower than the current level.");
                        DisplayContinuePrompt();
                    }
                }
                //If not a number
                else
                {
                    Console.WriteLine("Value not recognized, make sure your enturing a numerical value (2,17,235)");
                    DisplayContinuePrompt();
                }

            return (minMaxThreashold, MinMax);
        }

        #endregion

        /// <summary>
        /// Menu Option for Set Time
        /// </summary>
        /// <returns></returns>
        static int AlarmSystemDisplaySetTime()
        {
            int timeToMonitor = 0;
            string dummy;
            bool time;

            //Checking User Value
            do
            {
                time = true;
                DisplayScreenHeader("Set Time to Monitor");
                Console.WriteLine("\tEnter the number of seconds to test for the threshold value");
                dummy = Console.ReadLine().ToLower();

                if (int.TryParse(dummy, out timeToMonitor))
                {
                    if (timeToMonitor <= 0)
                    {
                        Console.WriteLine("The valued entered must be more than 0");
                        time = false;
                        DisplayContinuePrompt();
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine($"\t\tYou entered {timeToMonitor} seconds");
                    }
                }
                else
                {
                    Console.WriteLine("Value not recognized, make sure your enturing a numerical value (2,17,235)");
                    time = false;
                    DisplayContinuePrompt();
                }
            } while (!time);
            DisplayMenuPrompt("Alarm System");
            return timeToMonitor;
        }

        #region ALARM SET OPTION

        /// <summary>
        /// Menu option for Alarm System
        /// </summary>
        /// <param name="alarmData"></param>
        static void AlarmSystemDisplayAlarmSet(string sensorSelection, string[] rangeType, int[] minMaxThreshold, int timeToMonitor, Finch cornBread, string dataType)
        {
            int length = 0;
            string trigger = "null";
            int[] display = new int[2];
            int timeDisplay;

            DisplayScreenHeader("Set Alarm");

            //
            //Display Value Type
            //
            if (dataType == "both")
            {
                Console.WriteLine($"Selected Sensor:                  {sensorSelection} and Tempature");
                Console.WriteLine($"Range being Tested for Temp:      {rangeType[0]}");
                Console.WriteLine($"Range being Tested for Light:     {rangeType[1]}");
                Console.WriteLine($"Tempature value being Tested for: {minMaxThreshold[0]}");
                Console.WriteLine($"Light value being Tested for:     {minMaxThreshold[1]}");

            }
            else if (dataType == "light")
            {
                Console.WriteLine($"Selected Sensor:                  {sensorSelection}");
                Console.WriteLine($"Range being Tested for:           {rangeType[0]}");
                Console.WriteLine($"Value being Tested for:           {minMaxThreshold[0]}");
            }
            else
            {
                Console.WriteLine($"Selected Sensor:                  Tempature");
                Console.WriteLine($"Range being Tested for:           {rangeType[0]}");
                Console.WriteLine($"Value being Tested for:           {minMaxThreshold[0]}");
            }

                Console.WriteLine($"Duration of test:                 {timeToMonitor} seconds");
                Console.WriteLine();
                Console.WriteLine("\t\tPress Any Key to Begin");
                Console.ReadKey();

            //
            //Running Alarm
            //
            bool Alarm;                                                                                                  
            do
            {
                Alarm = false;

                //Getting Data
                (display, timeDisplay) = AlarmSystemDisplayAlarmRun(sensorSelection, minMaxThreshold, timeToMonitor, cornBread, length, dataType);
                
                //Checking Data
                trigger = AlarmSystemDisplayAlarmCheck(display, rangeType, minMaxThreshold, timeDisplay, cornBread, dataType);

                //Checking Trigger
                if (trigger == "Tripped" || trigger == "Time")
                {
                    Alarm = true;
                }
                //Continueing Alarm
                cornBread.wait(250);
                length = length + 1;
            } while (!Alarm);

            //
            //Testing to see if there is a reason to stop
            //
            if(trigger == "Tripped")
            {
                //
                //Alarm has been Triggered
                //
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine();
                if (dataType == "both")
                {
                    Console.WriteLine($"\t{rangeType[0]} or {rangeType[1]} Value Exceded");
                }
                else
                {
                    Console.WriteLine($"\t{rangeType[0]} Value Exceded");
                }
                Console.WriteLine();
                Console.WriteLine("\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                for (int i = 0; i < 3; i++)
                {
                    cornBread.noteOn(480);
                    cornBread.wait(200);
                    cornBread.noteOn(590);
                    cornBread.wait(100);
                }
                cornBread.noteOff();
            }
            else
            {
                //
                //Time has run out
                //
                Console.WriteLine();
                Console.WriteLine("\tTime's Up");
                Console.WriteLine("\tLevel not Exceeded");
            }

            DisplayMenuPrompt("Alarm System");
        }

        /// <summary>
        /// Getting Data for Alarm
        /// </summary>
        /// <param name="sensorSelection"></param>
        /// <param name="rangeType"></param>
        /// <param name="minMaxThreshold"></param>
        /// <param name="timeToMonitor"></param>
        /// <param name="cornbread"></param>
        static (int[], int) AlarmSystemDisplayAlarmRun(string sensorSelection, int[] minMaxThreshold, int timeToMonitor, Finch cornBread, int length, string dataType)
        {
            int[] display = new int[2];
            int timeDisplay = 0;
            int[] displayArray;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

            //Deciding what to display
            switch (dataType)
            {
                case "light":

                    //Get Light Value
                    switch (sensorSelection)
                    {
                        case "right":
                            display[0] = cornBread.getRightLightSensor();
                            break;

                        case "left":
                            display[0] = cornBread.getLeftLightSensor();
                            break;

                        case "both":
                            displayArray = cornBread.getLightSensors();
                            display[0] = (int)displayArray.Average();
                            break;
                    }
                    
                    //Display Light Values
                    Console.WriteLine($"\tCurrent Light Value: {display[0]}");
                    break;

                case "tempature":

                    //Get Tempature Values
                    display[0] = (int)cornBread.getTemperature();

                    //Display Tempature Values
                    Console.WriteLine($"\tCurrent Tempature Value: {display[0]}");
                    break;

                case "both":
                    //Get Values
                    display[0] = (int)cornBread.getTemperature();
                    displayArray = cornBread.getLightSensors();
                    display[1] = (int)displayArray.Average();

                    //Display Values
                    Console.WriteLine($"\tCurrent Tempature Value: {display[0]}");
                    Console.WriteLine($"\tCurrent Light Value: {display[1]}");
                    break;

            }
            // Calculating Time      
            timeDisplay = timeToMonitor - (length / 4);

            //Display Remaining Values
            if (dataType == "both")
            {
                Console.WriteLine($"\tSearching for Tempature Value: {minMaxThreshold[0]}");
                Console.WriteLine($"\tSearching for Light Value: {minMaxThreshold[1]}");
            }
            else
            {
                Console.WriteLine($"\tSearching for Value: {minMaxThreshold[0]}");
            }
            Console.WriteLine($"\tTime Remaining: {timeDisplay} seconds");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

            return (display, timeDisplay);
        }

        /// <summary>
        /// Checking the Alarm for Tripped Values
        /// </summary>
        /// <param name="sensorSelection"></param>
        /// <param name="rangeType"></param>
        /// <param name="minMaxThreshold"></param>
        /// <param name="timeToMonitor"></param>
        /// <param name="cornbread"></param>
        static string AlarmSystemDisplayAlarmCheck(int[] display, string[] rangeType, int[] minMaxThreshold, int timeDisplay, Finch cornbread, string dataType)
        {
            string trigger = "null";
            //
            //Maximum/Minimum Tests
            //
            if (dataType == "both")
            {
                for (int index = 0; index < 2; index++)
                {
                    if (rangeType[index] == "minimum")
                    {
                        if (display[index] < minMaxThreshold[index])
                        {
                            trigger = "Tripped";
                        }
                    }
                    else if (rangeType[index] == "maximum")
                    {
                        if (display[index] > minMaxThreshold[index])
                        {
                            trigger = "Tripped";
                        }
                    }
                }
            }
            else
            {
                if (rangeType[0] == "minimum")
                {
                    if (display[0] < minMaxThreshold[0])
                    {
                        trigger = "Tripped";
                    }
                }
                else if (rangeType[0] == "maximum")
                {
                    if (display[0] > minMaxThreshold[0])
                    {
                        trigger = "Tripped";
                    }
                }
            }
            //
            //Time Test
            //
            if (timeDisplay <= 0)
            {
                trigger = "Time";
            }

            return trigger;
        }

        #endregion


        #endregion

        #endregion

        #region DATA RECORDER

        #region DATA RECORDER MENU
        //
        //The menu check became cumbersome so I made its own regin
        //

        /// <summary>
        /// Data Recorder Menu
        /// </summary>
        /// <param name="cornBread"></param>
        /// <param name="numDone"></param>
        /// <param name="frequencyDone"></param>
        /// <param name="tempDone"></param>
        static void DataRecorderDisplayMenuScreen(Finch cornbread, string dummyUser)
        {
            Console.CursorVisible = true;

            double[] dataSet = null;
            string dummy;
            bool quitDataRecorderMenu = false;
            string menuChoice;
            (int, double) dataRecordingSpecifics = (0, 0);
            (int, int, int, string) dataRecordingIdentifications = (0, 0, 0, "null");
            var (numberDone, frequencyDone, tempDone, dataType) = dataRecordingIdentifications;
            var (numberOfDataPoints, dataPointFrequency) = dataRecordingSpecifics;

            do
            {
                DisplayScreenHeader("Data Recorder Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Number of Data Points");
                Console.WriteLine("\tb) Data Point Frequency");
                Console.WriteLine("\tc) Get Data");
                Console.WriteLine("\td) Display Data");
                Console.WriteLine("\te) Read Saved Data");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");

                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        numberOfDataPoints = DataRecorderDisplayGetNumberOfDataPoints();
                        numberDone = 1;
                        break;

                    case "b":
                        dataPointFrequency = DataRecorderDisplayGetDataPointFrequency();
                        frequencyDone = 1;
                        break;

                    case "c":
                        tempDone = 1;
                        dummy = DataRecorderMenuTest(numberDone, frequencyDone, tempDone);
                        if (dummy == "1")
                        {
                            dataType = DataRecorderDisplayGetDataType(numberOfDataPoints, dataPointFrequency);
                            dataSet = DataRecorderDisplayGetData(numberOfDataPoints, dataPointFrequency, dataType, cornbread);
                            tempDone = 2;
                        }
                        else
                        {
                            tempDone = 0;
                        }
                        break;

                    case "d":
                        DataRecorderMenuTest(numberDone, frequencyDone, tempDone);
                        if (tempDone == 2)
                        {
                            DataRecorderDisplayDataTable(dataSet, dataType);
                            DataRecorderFileSave(dataSet, dataType, dummyUser);
                        }
                        break;

                    case "e":
                        break;
                    case "q":
                        quitDataRecorderMenu = true;
                        DisplayMenuPrompt("Main Menu");
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitDataRecorderMenu);
        }

        /// <summary>
        /// Testing for entered values for menu
        /// </summary>
        /// <param name="numberDone"></param>
        /// <param name="frequencyDone"></param>
        /// <param name="tempDone"></param>
        /// <param name="cornbread"></param>
        static string DataRecorderMenuTest(int numberDone, int frequencyDone, int tempDone)
        {
            Console.Clear();
            string dummy = "0";
            //
            //Checking Menu Option Selected
            //
            if (tempDone == 1)
            {
                //
                //Get Data Selected
                //
                if ((numberDone != 1) && (frequencyDone != 1))
                {
                    DisplayScreenHeader("Get Data");
                    Console.WriteLine();
                    Console.WriteLine("\tUnable to record data");
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\tMissing information in menu options: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\tNumber of Data Points ");
                    Console.WriteLine("\tData Point Frequency");
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Black;
                    DisplayMenuPrompt("Data Recorder");
                }
                else if (numberDone != 1)
                {
                    DisplayScreenHeader("Get Data");
                    Console.WriteLine();
                    Console.WriteLine("\tUnable to record data");
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\tMissing information in menu options: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\tNumber of Data Points ");
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Black;
                    DisplayMenuPrompt("Data Recorder");
                }
                else if (frequencyDone != 1)
                {
                    DisplayScreenHeader("Get Data");
                    Console.WriteLine();
                    Console.WriteLine("\tUnable to record data");
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\tMissing information in menu options: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\tData Point Frequency");
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Black;
                    DisplayMenuPrompt("Data Recorder");
                }
                else if (frequencyDone == 1 && numberDone == 1)
                {
                    dummy = "1";
                }
            }
            //
            // Display Data Selected
            //
            else
            {
                if (tempDone != 2)
                {
                    DisplayScreenHeader("Display Data");
                    Console.WriteLine();
                    Console.WriteLine("\tThere is no recroded data to be displayed");
                    DisplayMenuPrompt("Data Recorder");
                }
            }
            return dummy;
        }

        #endregion

        #region DATA RECORDER OPTIONS

        /// <summary>
        /// Gets user responce and sets it as frequency
        /// </summary>
        /// <returns></returns>
        static double DataRecorderDisplayGetDataPointFrequency()
        {
            string dummy;
            double dataPointFrequency;
            DisplayScreenHeader("Data Point Frequency:");

            //Valadate Number
            bool freqNum;
            do
            {
                freqNum = true;
                Console.WriteLine("Frequency of Data Points");
                dummy = Console.ReadLine();
                if (!double.TryParse(dummy, out dataPointFrequency))
                {
                    Console.WriteLine();
                    Console.WriteLine("Ensure the value is written numerically (1.7, 7.43, 345.63).");
                    freqNum = false;
                }
            } while (!freqNum);

            Console.WriteLine();
            Console.WriteLine($"You chose {dataPointFrequency} as the frequency of data prompts");
            DisplayMenuPrompt("Data Recorder");

            return dataPointFrequency;
        }

        /// <summary>
        /// Gets user responce and sets it as number of data points
        /// </summary>
        /// <returns></returns>
        static int DataRecorderDisplayGetNumberOfDataPoints()
        {
            string dummy;
            int numberOfDataPoint;
            DisplayScreenHeader("Number of Data Points");

            //Valadate Number
            bool dataNum;
            do
            {
                dataNum = true;
                Console.WriteLine("Number of Data Points:");
                dummy = Console.ReadLine();
                if (!int.TryParse(dummy, out numberOfDataPoint))
                {
                    Console.WriteLine();
                    Console.WriteLine("Ensure the value is written numerically (1, 7, 345).");
                    dataNum = false;
                }
            } while (!dataNum);

            Console.WriteLine();
            Console.WriteLine($"You chose {numberOfDataPoint} as the number of data prompts");
            DisplayMenuPrompt("Data Recorder");

            return numberOfDataPoint;
        }

        /// <summary>
        /// Uses number and frequency to get tempature data
        /// </summary>
        /// <param name="numberOfDataPoints"></param>
        /// <param name="dataPointFrequency"></param>
        /// <param name="cornBread"></param>
        /// <returns></returns>
        static string DataRecorderDisplayGetDataType(int numberOfDataPoints, double dataPointFrequency)
        {
            string dummy;
            bool dataChoice;
            string dataType = "null";
            do
            {
                dataChoice = true;
                DisplayScreenHeader("Get Data");
                Console.WriteLine("\tWhat data type would you like to record");
                Console.WriteLine("\tTempature or Light");
                dummy = Console.ReadLine().ToLower();
                if (dummy != "tempature" && dummy != "light")
                {
                    Console.WriteLine();
                    Console.WriteLine("\tEntry not recognized, please enter one of the two options above");
                    DisplayContinuePrompt();
                    dataChoice = false;
                }
            } while (!dataChoice);

            switch (dummy)
            {
                case "tempature":
                    DisplayScreenHeader("Tempatures");
                    Console.WriteLine($"The finch robot will now record {numberOfDataPoints} data points {dataPointFrequency} seconds apart");
                    Console.WriteLine("Press any key to begin");
                    Console.ReadKey();
                    dataType = "Tempature";
                    break;

                case "light":
                    DisplayScreenHeader("Light");
                    Console.WriteLine($"The finch robot will now record {numberOfDataPoints} data points {dataPointFrequency} seconds apart");
                    Console.WriteLine("Press any key to begin");
                    Console.ReadKey();
                    dataType = "Light";
                    break;
            }
            return dataType;
        }

        /// <summary>
        /// Uses number and frequency to get tempature data
        /// </summary>
        /// <param name="numberOfDataPoints"></param>
        /// <param name="dataPointFrequency"></param>
        /// <param name="cornBread"></param>
        /// <returns></returns>
        static double[] DataRecorderDisplayGetData(int numberOfDataPoints, double dataPointFrequency, string dataType, Finch cornBread)
        {
            double[] dataSet = new double[numberOfDataPoints];
            double tempC;
            double tempF;

            switch (dataType)
            {
                case "Tempature":
                    for (int index = 0; index < numberOfDataPoints; index++)
                    {
                        cornBread.noteOn(1200);
                        cornBread.wait(250);
                        cornBread.noteOff();
                        tempC = cornBread.getTemperature();
                        tempF = DataRecorderConvertion(tempC);
                        dataSet[index] = tempF;
                        Console.WriteLine($"Tempature {index + 1}: {dataSet[index]:n1}");
                        cornBread.wait((int)dataPointFrequency * 1000);
                    }
                    break;

                case "Light":
                    for (int index = 0; index < numberOfDataPoints; index++)
                    {
                        cornBread.noteOn(1200);
                        cornBread.wait(250);
                        cornBread.noteOff();
                        int[] Light = cornBread.getLightSensors();
                        dataSet[index] = Light.Average();
                        Console.WriteLine($"Light Level {index + 1}: {dataSet[index]:n1}");
                        cornBread.wait((int)dataPointFrequency * 1000);
                    }
                    break;
            }

            //
            //display Table
            //
            cornBread.noteOn(1000);
            cornBread.wait(100);
            cornBread.noteOn(1500);
            cornBread.wait(100);
            cornBread.noteOff();
            DataRecorderDisplayDataTable(dataSet, dataType);
            return dataSet;
        }

        /// <summary>
        /// Converts Farienhight to Celcius
        /// </summary>
        /// <param name="tempC"></param>
        /// <returns></returns>
        static double DataRecorderConvertion(double tempC)
        {
            double tempF;
            tempF = ((tempC * 9) / 5) + 32;
            return tempF;
        }

        /// <summary>
        /// Displaying Tempature Values
        /// </summary>
        /// <param name="dataSet"></param>
        static void DataRecorderDisplayDataTable(double[] dataSet, string dataType)
        {
            Console.WriteLine("");
            Console.WriteLine("" +
                "Reading #".PadLeft(20) +
                $"{dataType}".PadLeft(15)
                );
            Console.WriteLine("----------------------------------------------");
            for (int index = 0; index < dataSet.Length; index++)
            {
                Console.WriteLine(""+
                    $"{index + 1}".PadLeft(20) +
                    $"{dataSet[index]:n1}".PadLeft(15)
                );
            }
            DisplayMenuPrompt("Data Recorder");
        }

        static void DataRecorderFileSave(double[] dataSet, string dataType, string dummyUser)
        {
            bool Choice = false;
            string dataPath = "null";
            Console.WriteLine();
            Console.WriteLine("\tWould you like to save this Data: (Y/N)");
            do
            {
                if (Console.ReadLine().ToLower() == "y")
                {
                    Choice = true;
                    switch (dummyUser)
                    {
                        case "User1":
                            dataPath = @"C:\Finch-Control-master\Project_FinchControl\Data\User(1)Data.txt";
                            break;
                        case "User2":
                            dataPath = @"C:\Finch-Control-master\Project_FinchControl\Data\User(2)Data.txt";
                            break;
                        case "User3":
                            dataPath = @"C:\Finch-Control-master\Project_FinchControl\Data\User(3)Data.txt";
                            break;
                        case "Admin":
                            do
                            {
                                Choice = true;
                                Console.WriteLine("Which file would you like to save over: (1,2,3)");
                                switch (Console.ReadLine())
                                {
                                    case "1":
                                        dataPath = @"C:\Finch-Control-master\Project_FinchControl\Data\User(1)Data.txt";
                                        break;
                                    case "2":
                                        dataPath = @"C:\Finch-Control-master\Project_FinchControl\Data\User(2)Data.txt";
                                        break;
                                    case "3":
                                        dataPath = @"C:\Finch-Control-master\Project_FinchControl\Data\User(3)Data.txt";
                                        break;
                                    default:
                                        Choice = false;
                                        Console.WriteLine("Entered Value not Recognized");
                                        break;
                                }
                            } while (!Choice);
                            break;
                    }
                }
                else if (Console.ReadLine().ToLower() == "n")
                {
                    Choice = true;
                }
                else
                {
                    Console.WriteLine("Entry not recognized");
                    DisplayContinuePrompt();
                }
            } while (!Choice);
        }
        #endregion

        #endregion

        #region TALENT SHOW

        /// <summary>
        /// *****************************************************************
        /// *                     Talent Show Menu                          *
        /// *****************************************************************
        /// </summary>
        static void TalentShowDisplayMenuScreen(Finch cornBread)
        {
            Console.CursorVisible = true;

            bool quitTalentShowMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Light and Sound");
                Console.WriteLine("\tb) Dance");
                Console.WriteLine("\tc) Mix It Up");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        TalentShowDisplayLightAndSound(cornBread);
                        break;

                    case "b":
                        TalentShowDisplayDance(cornBread);
                        break;

                    case "c":
                        TalentShowDisplayMixItUp(cornBread);
                        break;

                    case "q":
                        quitTalentShowMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitTalentShowMenu);
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Mix It Up                  *
        /// *****************************************************************
        /// </summary>
        /// <param name="cornBread">finch robot object</param>
        private static void TalentShowDisplayMixItUp(Finch cornBread)
        {
            Console.CursorVisible = false;
            DisplayScreenHeader("Mix It Up");

            Console.WriteLine("\tThe Finch robot will get a little crazy!");
            Console.WriteLine();
            Console.WriteLine("\t(Make sure there is space around the Finch)");
            DisplayContinuePrompt();

            //
            //Begin Show
            //
            Console.WriteLine("");
            Console.WriteLine("  ░░                  ░░ ");
            Console.WriteLine("  ░░                  ░░ ");
            Console.WriteLine("  ░░                  ░░ ");
            Console.WriteLine("  ░░        ░░░░      ░░ ");
            Console.WriteLine("  ░░      ▒▒░░░░▒▒    ░░ ");
            Console.WriteLine("    ▓▓    ░░░░░░▒▒  ▓▓   ");
            Console.WriteLine("    ▓▓▓▓  ▒▒░░▒▒▒▒▒▒▓▓   ");
            Console.WriteLine("      ▓▓▒▒▓▓░░▓▓▒▒▓▓     ");
            Console.WriteLine("      ░░▓▓▓▓▒▒▓▓▓▓░░     ");
            Console.WriteLine("        ░░▓▓▒▒▓▓▓▓       ");
            Console.WriteLine("          ▓▓▒▒▒▒░░       ");
            Console.WriteLine("          ▒▒▓▓▒▒         ");
            Console.WriteLine("          ▒▒▓▓▓▓         ");
            Console.WriteLine("          ▒▒▒▒▒▒         ");
            Console.WriteLine("        ▒▒▒▒  ▒▒░░       ");
            Console.WriteLine("      ░░▒▒      ▒▒░░     ");
            Console.WriteLine("      ▒▒          ▒▒▒▒   ");
            Console.WriteLine("    ▒▒▒▒            ▒▒▒▒ ");
            Console.WriteLine("    ▒▒                ▒▒ ");
            Console.WriteLine("    ▒▒▒▒              ▒▒  ");
            Console.WriteLine("      ▒▒              ▒▒  ");
            Console.WriteLine("      ▒▒              ▒▒  ");
            Console.WriteLine("    ▓▓▓▓              ▓▓▓▓");

            cornBread.setMotors(100, 0);
            Showtime(cornBread);

            //
            //Reset
            //
            cornBread.setMotors(0, 0);

            DisplayMenuPrompt("Talent Show Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Dance                 *
        /// *****************************************************************
        /// </summary>
        /// <param name="cornBread">finch robot object</param>
        private static void TalentShowDisplayDance(Finch cornBread)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Dance");

            Console.WriteLine("\tThe Finch robot will not show off its fancy feet!");
            Console.WriteLine();
            Console.WriteLine("\t(Make sure there is space around the Finch)");
            DisplayContinuePrompt();

            //
            // Begin Dance
            //
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.WriteLine("\tOff We Go... " +
                "Make sure the cord does not get tangeled");
            Console.WriteLine();
            Console.WriteLine("$$$$$$$$$$$$R$$$$$$$$$$$$$$$$$");
            Console.WriteLine("$$$$$$$V 'Y .''**##%%$$$$$$$$$");
            Console.WriteLine("$$$$# `$$N$$$$$$$$$mmmuuu:.''#");
            Console.WriteLine("$$R    $$$$$$$$$$$$$$$$$$$$$$$");
            Console.WriteLine("$!   @$$$$$''R$$$$$$$$$$$$$$$$");
            Console.WriteLine("$> '$$$$$F     $$$$$$$$$$$$$$$");
            Console.WriteLine("$W   $$$$:     R$$$$$$$$$$$$$$");
            Console.WriteLine("$$     $$8$$R: x@$$$$$$$$$$$$$");
            Console.WriteLine("$$$      $$$$x$$$$$$$$$$$$$$$$");
            Console.WriteLine("$$$     $N$ '$$$$$$$$$$$$$$$$$");
            Console.WriteLine("$$     $$$$ @  $$$$$$$$$$$$$$$");
            Console.WriteLine("$`   u$$$$$    R$$$$$$$$$$$$$$");
            Console.WriteLine("   N$R$$$$      R$$$$$$$$$$$$$");
            Console.WriteLine("  *$$@$$$f.i.   `$$$$$$$$$$$$$");
            Console.WriteLine("k          9$$$.   $$$$$$$$$$$");
            Console.WriteLine("R          M$$$$$.   $$$$$$$$$");
            Console.WriteLine("$          4$$$$$$$.  $$$$$$$$");
            Console.WriteLine("$          @$$$$$$$$$b-B>R$$$$");
            Console.WriteLine("$          $$$$$$$$$$$$$$$$$$$");
            Console.WriteLine("$ <        $$$$$$$$$$$$$$$$$$$");
            Console.WriteLine("$ $b      $$$$$$$$$$$$$$$$$$$$");
            Console.WriteLine("$$$$i     $$$$$$$$$$$$$$$$$$$$");
            Console.WriteLine("$$$$$c    #$$$$$$$$$$$$$$$$$$$");
            Console.WriteLine("$$$$$R     ?$$$$$$$$$$$$$$$$$$");
            Console.WriteLine("$$$$$$      $$$$$$$$$$$$$$$$$$");
            Console.WriteLine("$$$$$$       $$$$$$$$$$$$$$$$$");
            Console.WriteLine("$$$$$$  .i.   #$$$$$$$$$$$$$$$");
            Console.WriteLine("$$$$$R  d$b    M$$$$$$$$$$$$$$");
            Console.WriteLine("$$$$$r  $$$$$$   9$$$$$$$$$$$$");
            Console.WriteLine("$$$$#  X$$$$$$$od$$$$$$$$$$$$$");
            Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");

            cornBread.setMotors(100, 0);
            cornBread.wait(2000);
            cornBread.setMotors(-100, 0);
            cornBread.wait(2000);
            cornBread.setMotors(255, 255);
            cornBread.wait(200);
            cornBread.setMotors(-100, -100);
            cornBread.wait(400);

            //
            //reset
            //
            cornBread.setMotors(0, 0);

            DisplayMenuPrompt("Talent Show Menu");
            SetTheme();
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Light and Sound                   *
        /// *****************************************************************
        /// </summary>
        /// <param name="cornBread">finch robot object</param>
        static void TalentShowDisplayLightAndSound(Finch cornBread)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Light and Sound");

            Console.WriteLine("\tThe Finch robot will not show off its glowing talent!");
            DisplayContinuePrompt();

            Showtime(cornBread);

            DisplayMenuPrompt("Talent Show Menu");
        }

        /// <summary>
        /// The Song of the Angels and Lights of the Gods
        /// </summary>
        /// <param name="cornBread"></param>
        static void Showtime(Finch cornBread)
        {
            bool Note;
            string dummy;
            Int32 note;

            Console.WriteLine();
            Console.WriteLine("The Finch needs a little help, what note (in Hz) should be the big finish?");
            Console.CursorVisible = true;
            do
            {
                Note = false;
                Console.WriteLine("Note:");
                dummy = Console.ReadLine();
                if (Int32.TryParse(dummy, out note))
                {
                    Note = true;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("The note must be written numerically, (1, 89, 1,047).");
                    Console.WriteLine();
                }
            } while (!Note);

            for (int enough = 0; enough <= 2; enough++)
            {
                cornBread.noteOn(294);
                cornBread.wait(175);
                cornBread.noteOn(587);
                cornBread.setLED(255, 255, 255);
                cornBread.wait(175);
                cornBread.noteOn(294);
                cornBread.wait(175);
                cornBread.noteOn(659);
                cornBread.setLED(255, 100, 100);
                cornBread.wait(175);
                cornBread.noteOn(294);
                cornBread.wait(175);
                cornBread.noteOn(587);
                cornBread.setLED(255, 255, 255);
                cornBread.wait(175);
                cornBread.noteOn(294);
                cornBread.wait(175);
                cornBread.noteOn(523);
                cornBread.setLED(100, 100, 255);
                cornBread.wait(175);
            }
            cornBread.noteOff();

            Console.WriteLine();
            Console.WriteLine("Here the big finish");
            Console.WriteLine();
            cornBread.wait(1000);
            cornBread.noteOn(note);
            cornBread.wait(2000);

            cornBread.noteOff();
        }

        #endregion

        #region FINCH ROBOT MANAGEMENT

        /// <summary>
        /// *****************************************************************
        /// *               Disconnect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="cornBread">finch robot object</param>
        static void DisplayDisconnectFinchRobot(Finch cornBread)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("\tAbout to disconnect from the Finch robot.");
            DisplayContinuePrompt();

            cornBread.disConnect();

            Console.WriteLine("\tThe Finch robot is now disconnect.");

            DisplayMenuPrompt("Main Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *                  Connect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="cornBread">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static bool DisplayConnectFinchRobot(Finch cornBread)
        {
            Console.CursorVisible = false;

            bool robotConnected;

            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
            DisplayContinuePrompt();

            do
            {
                Console.WriteLine();
                Console.WriteLine("Testing Now...");
                Console.WriteLine();
                robotConnected = cornBread.connect();

                if (cornBread.connect())
                {
                    Console.WriteLine();
                    Console.WriteLine("\tFinch Connection Sucsesful");

                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\tFinch Not Detected");
                    Console.WriteLine();
                    Console.WriteLine("\tRe-check the conection and press any key to retry");
                    Console.ReadKey();
                }
            }
            while (!robotConnected);

            //
            // sucsesful ding
            //
            cornBread.noteOn(1000);
            cornBread.setLED(255, 0, 0);
            cornBread.wait(400);
            cornBread.noteOn(294);
            cornBread.setLED(255, 0, 255);
            cornBread.wait(200);
            cornBread.noteOn(1500);
            cornBread.setLED(255, 255, 255);
            cornBread.wait(300);

            //
            // reset finch robot
            //
            cornBread.setLED(0, 0, 0);
            cornBread.noteOff();

            DisplayMenuPrompt("Main Menu");
            return robotConnected;
        }

        static void TestingForConnection(bool robotConnected)
        {
            if (robotConnected == false)
            {
                Console.WriteLine();
                Console.WriteLine("No finch robot detected");
                Console.WriteLine("ensure you have connected your finch robot before acsessing this menu.");
                DisplayMenuPrompt("Main Menu");
            }
        }
        #endregion

        #region USER INTERFACE

        /// <summary>
        /// *****************************************************************
        /// *                     Welcome Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tWelcome to Finch Control");
            Console.WriteLine();
            Console.WriteLine("\tHere you will be able to connect, control, and utilize a Finch Robot");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Closing Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display menu prompt
        /// </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        /// <summary>
        /// setup the console theme
        /// </summary>
        static void SetTheme()
        {
            string dummy;
            ConsoleColor backgroundColor = ConsoleColor.Black;
            ConsoleColor forgroundColor = ConsoleColor.White;
            string dataPath = @"C:\Finch-Control-master\Project_FinchControl\Data\Theme.txt";

            try
            {
                string[] Theme = File.ReadAllLines(dataPath);
                if (Theme.Length <= 2 && Enum.TryParse<ConsoleColor>(Theme[1], out backgroundColor) && Enum.TryParse<ConsoleColor>(Theme[0], out forgroundColor))
                {
                    bool Keep = false;
                    do
                    {
                        Console.ForegroundColor = forgroundColor;
                        Console.BackgroundColor = backgroundColor;
                        DisplayScreenHeader("Theme Selection");
                        Console.WriteLine("\tThe Theme is currently set to");

                        Console.WriteLine($"\t\tForground: {forgroundColor}");
                        Console.WriteLine($"\t\tBackground: {backgroundColor}");
                        Console.WriteLine();

                        Console.WriteLine("\tWould you like to keep this setting? (Y/N)");
                        dummy = Console.ReadLine().ToLower();
                        if (dummy == "y" || dummy == "n")
                        {
                            if (dummy == "y")
                            {
                                Keep = true;
                            }
                            else
                            {
                                Console.WriteLine("\t\tPlease enter your chosen colors");
                                Console.WriteLine();
                                bool Color = false;
                                do
                                {
                                    Console.WriteLine("Forground Color: ");
                                    string color = Console.ReadLine().ToLower();
                                    if (Enum.TryParse<ConsoleColor>(color, true, out forgroundColor))
                                    {
                                        Color = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Color not recognized, please enter a valid color");
                                    }

                                } while (!Color);
                                Console.WriteLine();
                                do
                                {
                                    Console.WriteLine("Background Color: ");
                                    string color = Console.ReadLine();
                                    if (Enum.TryParse<ConsoleColor>(color, true, out backgroundColor))
                                    {
                                        Color = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Color not recognized, please enter a valid color");
                                    }

                                } while (!Color);
                            }
                        }
                        else
                        {
                            Console.WriteLine("The responce is not recognized");
                            DisplayContinuePrompt();
                        }

                    } while (!Keep);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("The file Theme.txt has extra data");
                    Console.WriteLine("Please redownload Theme.txt and re-launch the program to acsess this function");
                }
                Theme[0] = forgroundColor.ToString();
                Theme[1] = backgroundColor.ToString();
                File.WriteAllLines(dataPath, Theme);
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine();
                Console.WriteLine("The file Theme.txt is missing data");
                Console.WriteLine("Please redownload Theme.txt and re-launch the program to acsess this function");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine();
                Console.WriteLine("The file Theme.txt is missing from the folder Data");
                Console.WriteLine("Please redownload Theme.txt and place it in the folder and re-launch the program to acsess this function");
            }
            finally
            {
                DisplayContinuePrompt();
            }
        }
        #endregion
    }
}
