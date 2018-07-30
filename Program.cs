using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Runtime.CompilerServices;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/

enum PlayerAction
{
    WAIT = 0,
    MOVE = 1,
    BUILD = 2,
    TRAIN = 3
}

public class Position
{
    public int x, y;
}

#region IActions

public abstract class IAction
{
    public abstract string ToString_Impl();
    public string message;

    protected IAction(string message = "")
    {
        this.message = message;
    }

    public override string ToString()
    {
        if (message != "")
        {
            return ToString_Impl() +" "+ message;
        }
        else
        {
            return ToString_Impl();
        }

    }
}

public class TurnAction : IAction
{
    public List<IAction> actions;

    public TurnAction()
    {
        this.message = "";
    }
    
    public TurnAction(string message) : base(message)
    {
        this.message = "";
    }

    public override string ToString_Impl()
    {
        string result = "";
        foreach (var action in actions)
        {
            result += action.ToString();
            result += ";";
        }

        return result;
    }
}

public class Train : IAction
{
    public string actionName = "TRAIN";
    public int siteId;

    public override string ToString_Impl()
    {
        return this.actionName + " "+ this.siteId;
    }
}

public class Wait : IAction
{
    public string actionName = "WAIT";
    
    public override string ToString_Impl()
    {
        return this.actionName;
    }
}

public class Move : IAction
{
    public string actionName = "MOVE";
    public Position targetPos; 
    
    public override string ToString_Impl()
    {
        return actionName + " " + targetPos.x +" " + targetPos.y;
    }
}

public class Alexander
{
    public void ParseInputs()
    {
        string[] inputs;
        int numSites = int.Parse(Console.ReadLine());
        for (int i = 0; i < numSites; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int siteId = int.Parse(inputs[0]);
            int x = int.Parse(inputs[1]);
            int y = int.Parse(inputs[2]);
            int radius = int.Parse(inputs[3]);
        }

        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            int gold = int.Parse(inputs[0]);
            int touchedSite = int.Parse(inputs[1]); // -1 if none
            for (int i = 0; i < numSites; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int siteId = int.Parse(inputs[0]);
                int ignore1 = int.Parse(inputs[1]); // used in future leagues
                int ignore2 = int.Parse(inputs[2]); // used in future leagues
                int structureType = int.Parse(inputs[3]); // -1 = No structure, 2 = Barracks
                int owner = int.Parse(inputs[4]); // -1 = No structure, 0 = Friendly, 1 = Enemy
                int param1 = int.Parse(inputs[5]);
                int param2 = int.Parse(inputs[6]);
            }
            int numUnits = int.Parse(Console.ReadLine());
            for (int i = 0; i < numUnits; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int x = int.Parse(inputs[0]);
                int y = int.Parse(inputs[1]);
                int owner = int.Parse(inputs[2]);
                int unitType = int.Parse(inputs[3]); // -1 = QUEEN, 0 = KNIGHT, 1 = ARCHER
                int health = int.Parse(inputs[4]);
            }

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");


            // First line: A valid queen action
            // Second line: A set of training instructions
//            Console.WriteLine("WAIT");
//            Console.WriteLine("TRAIN");
        }
    }
    
    public string think()
    {
        List<IAction> chosenMove = new List<IAction>();
        
        
        // Default: No action found. Just wait
        var nullOrEmpty = chosenMove == null || !chosenMove .Any();
        if (nullOrEmpty)
        {
            chosenMove.Append(new Wait());
        }
        
        string result = "";
        foreach (var action in chosenMove)
        {
            result += action.ToString();
            result += ";";
        }
        
        return result;
    }
}

public class GameState
{
    public List<Sites> sites;
}

public class Sites
{
    public int siteId;
    public Position pos;
    public int radius;

    public Sites(int siteId, Position pos, int radius)
    {
        this.siteId = siteId;
        this.pos = pos;
        this.radius = radius;
    }
}

#endregion        

class Player
{
    static void Main(string[] args)
    {
        Alexander alexander = new Alexander();
        alexander.ParseInputs();
        string move = alexander.think();
        Console.WriteLine(move);
    }
}