using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Runtime.CompilerServices;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
public class Position
{
    public int x, y;

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return $"({x},{y})";
    }
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
    public IAction buildAction;
    public IAction trainAction;

    public TurnAction()
    {
        this.message = "";
        buildAction = new Wait();
        trainAction = new HaltTraing();
    }
    
    public TurnAction(string message) : base(message)
    {
        this.message = "";
    }

    public void PrintMove()
    {
        Console.WriteLine(buildAction.ToString());
        Console.WriteLine(trainAction.ToString());
    }

    public override string ToString_Impl()
    {
        return "Invalid";
    }

    public override string ToString()
    {
        return ToString_Impl();
    }
}

//
//public class TurnAction : IAction
//{
//    public List<IAction> buildActions;
//    public List<IAction> trainActions;
//
//    public TurnAction()
//    {
//        this.message = "";
//        buildActions = new List<IAction>(){new Wait()};
//        buildActions = new List<IAction>(){new HaltTraing()};
//        
//    }
//    
//    public TurnAction(string message) : base(message)
//    {
//        this.message = "";
//    }
//
//    public override string ToString_Impl()
//    {
//        string result = "";
//        foreach (var action in buildActions)
//        {
//            result += action.ToString();
//            result += ";";
//        }
//
//        result += "/n";
//        
//        foreach (var action in trainActions)
//        {
//            result += action.ToString();
//            result += ";";
//        }
//
//        return result;
//    }
//
//    public override string ToString()
//    {
//        return ToString_Impl();
//    }
//}

public class HaltTraing : IAction
{
    public string actionName = "TRAIN";
    
    public override string ToString_Impl()
    {
        return this.actionName;
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
    public GameState game;
    
    public void ParseInputs_Turn()
    {
        string[] inputs;

        inputs = Console.ReadLine().Split(' ');
        int gold = int.Parse(inputs[0]);
        int touchedSite = int.Parse(inputs[1]); // -1 if none
        for (int i = 0; i < game.numSites; i++)
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
    
    public TurnAction think()
    {
        TurnAction chosenMove = new TurnAction();
        
        return chosenMove;
    }

    public void ParseInputs_Begin()
    {
        game = new GameState();
        string[] inputs;
        game.numSites = int.Parse(Console.ReadLine());
        
        for (int i = 0; i < game.numSites; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int siteId = int.Parse(inputs[0]);
            int x = int.Parse(inputs[1]);
            int y = int.Parse(inputs[2]);
            int radius = int.Parse(inputs[3]);
            
            Site newSite = new Site(siteId, new Position(x,y), radius);
            game.sites.Add(newSite);
        }
    }
}

public class GameState
{
    public List<Site> sites = new List<Site>();
    public int numSites;
}

public class Site
{
    public int siteId;
    public Position pos;
    public int radius;

    public Site(int siteId, Position pos, int radius)
    {
        this.siteId = siteId;
        this.pos = pos;
        this.radius = radius;
    }

    public override string ToString()
    {
        return $"Site {siteId} - Pos: {pos} - Rad: {radius}";
    }
}

#endregion        

class Player
{
    static void Main(string[] args)
    {
        Alexander alexander = new Alexander();

        alexander.ParseInputs_Begin();
        
        while (true)
        {
            alexander.ParseInputs_Turn();

            Console.Error.WriteLine(alexander.game.sites.Aggregate("", (agg, x) => agg + "\n"+ x.ToString()));
            
            TurnAction move = alexander.think();
            move.PrintMove();
        }
    }
}