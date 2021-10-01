using System;
using System.Collections.Generic;

namespace IOOFRobotChallenge
{
    public enum Direction
    {
        NORTH,
        SOUTH,
        WEST,
        EAST
    }

    class RobotsOnATable
    {
        public static bool programFinished = false;
        static void Main()
        {

            Table robotTable = new Table(5, 5);
            while (programFinished == false)
            {
                string userInput = Console.ReadLine();
              
                if (userInput == "REPORT")
                {
                    Console.WriteLine(robotTable.Report());
                    programFinished = true;
                }
                else
                {
                    robotTable.Command(userInput);
                }
               
            }
        }

    }

    public class Robot 
    {
        //In this solution the robot class is a simple object that stores its position and direction. 
        //Moving and rotating in response to whatever commands are recieved by the table.
      public int xPosition;
      public int yPosition;
      public Direction direction;
      
      public Robot(int startingX, int startingY, Direction startingDirection, int id)
        {
            xPosition = startingX;
            yPosition = startingY;
            direction = startingDirection;
            
        }
      public void Move()
        {
            switch (direction)
            {
                case Direction.NORTH:
                    yPosition++;
                    break;

                case Direction.SOUTH:
                    yPosition--;
                    break;

                case Direction.EAST:
                    xPosition++;
                    break;

                case Direction.WEST:
                    xPosition--;
                    break;

            }
                    
        }
    
    public void Rotate(bool turningLeft)
       {
            //Switch statement to implement simple logic for turning around, perhaps working with actual angles as integers would be simpler.
            //Boolean used as a simple way to store left or right.
            switch (direction)
            {
                case Direction.NORTH:
                    if (turningLeft) { direction = Direction.WEST; }
                    else { direction = Direction.EAST; }
                    break;

                case Direction.SOUTH:
                    if (turningLeft) { direction = Direction.EAST; }
                    else { direction = Direction.WEST; }
                    break;

                case Direction.EAST:
                    if (turningLeft) { direction = Direction.NORTH; }
                    else { direction = Direction.SOUTH; }
                    break;

                case Direction.WEST:
                    if (turningLeft) { direction = Direction.SOUTH; }
                    else { direction = Direction.NORTH; };
                    break;

            }

        }

    }

    public class Table
    {
        
        public Table(int width, int height)
        {
            table_width = width;
            table_height = height;
            RobotList = new List<Robot>();
        }
        private int _activerobot;
        private List<Robot> _robotlist;
        private int table_width;
        private int table_height;

        public int ActiveRobot { get => _activerobot; set => _activerobot = value;}
        public List<Robot> RobotList { get => _robotlist; set => _robotlist = value; }

        public void Command(string input)
        {
            //Process user input for parsing into command arguements, assuming input will always be error free. 
            //If sanitzing input is required, Parse() could be changed into TryParse.

            if (input.StartsWith("PLACE")){

                //Remove the leading command name, white space and commas to be left with clean input
                input = input.Replace("PLACE", "").Trim();
                string[] stringArgs = input.Split(",");

                int placeX;
                int placeY;
                Direction placeDirection;

                placeX = int.Parse(stringArgs[0]);
                placeY = int.Parse(stringArgs[1]);
                placeDirection = (Direction) Enum.Parse(typeof(Direction), stringArgs[2]);

                PlaceRobot(placeX, placeY, placeDirection, RobotList.Count+1);

            } 

        if (input.StartsWith("ROBOT")){
                input = input.Replace("ROBOT", "").Trim();
                ActiveRobot = int.Parse(input) - 1;
            }

        if (input.StartsWith("MOVE"))
            { 
                //Store active robots information for later
                Direction attemptedDirection = RobotList[ActiveRobot].direction;
                int startingX = RobotList[ActiveRobot].xPosition;
                int startingY = RobotList[ActiveRobot].yPosition;
                // Check the current robots direction and position to see if the move should be ignored or not.

                switch (attemptedDirection)
                {
                    case Direction.NORTH:
                        if (startingY < table_height) { RobotList[ActiveRobot].Move(); }
                        break;

                    case Direction.SOUTH:
                        if (startingY > 0) { RobotList[ActiveRobot].Move(); }
                        break;

                    case Direction.EAST:
                        if (startingX < table_width) { RobotList[ActiveRobot].Move(); }
                        break;

                    case Direction.WEST:
                        if (startingX > 0) { RobotList[ActiveRobot].Move(); }
                        break;

                }
            }

        if (input.StartsWith("LEFT"))
            {
                RobotList[ActiveRobot].Rotate(true);
            }
        if (input.StartsWith("RIGHT"))
            {
                RobotList[ActiveRobot].Rotate(false);
            }
        }

        

        public void PlaceRobot(int x, int y, Direction startingDirection, int id)
        {
            RobotList.Add(new Robot(x, y, startingDirection, id));
        }

        public string Report()
        {
            //Produce and return a large string to inform the user about the state of the table.

            string RobotCount = RobotList.Count.ToString();
            string Report = "There is currently " + RobotCount + " robots on the table.\n";

            //To match the desired display behaviour, we simply increment the robot identifier at the report stage.
            int NowActive = ActiveRobot + 1;

            Report += NowActive.ToString() + " is the current active robot\n";
            Report += RobotList[ActiveRobot].xPosition.ToString() + "," + RobotList[ActiveRobot].yPosition.ToString() + "," + RobotList[ActiveRobot].direction.ToString();
            return Report;
        }
    }
}
