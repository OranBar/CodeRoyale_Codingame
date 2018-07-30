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

    public IAction(string message = "")
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
    public IAction queenAction;
    public IAction trainAction;

    public TurnAction()
    {
        this.message = "";
        queenAction = new Wait();
        trainAction = new HaltTraing();
    }
    
    public TurnAction(string message) : base(message)
    {
        this.message = "";
    }

    public void PrintMove()
    {
        Console.WriteLine(queenAction.ToString());
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

#region Train Actions

public class HaltTraing : IAction
{
    public string actionName = "TRAIN";
    
    public HaltTraing(string message = "") : base(message){}

    
    public override string ToString_Impl()
    {
        return this.actionName;
    }
}

public class Train : IAction
{
    public string actionName = "TRAIN";
    public int siteId;

    public Train(string message = "") : base(message){}
    
    public override string ToString_Impl()
    {
        return this.actionName + " "+ this.siteId;
    }
}

#endregion

#region QueenActions

public class Wait : IAction
{
    public string actionName = "WAIT";

    public Wait(string message = "") : base(message){}

    public override string ToString_Impl()
    {
        return this.actionName;
    }
}

public class Move : IAction
{
    public string actionName = "MOVE";
    public Position targetPos; 
    
    public Move(string message = "") : base(message){}
    
    public override string ToString_Impl()
    {
        return actionName + " " + targetPos.x +" " + targetPos.y;
    }
}

#endregion

public class LaPulzellaD_Orleans
{
    public GameInfo game;

    public GameState currentGameState;
    
    public void ParseInputs_Turn()
    {
        string[] inputs;

        currentGameState = new GameState();
        
        inputs = Console.ReadLine().Split(' ');
        currentGameState.gold = int.Parse(inputs[0]);
        currentGameState.touchedSite = int.Parse(inputs[1]); // -1 if none
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
            
            Site site = new Site(siteId, ignore1, ignore2, structureType, owner, param1, param2);
            currentGameState.sites.Add(site);
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
            
            Unit unit = new Unit(x,y,owner,unitType,health);
            currentGameState.units.Add(unit);
        }
    }
    
    public TurnAction think()
    {
        TurnAction chosenMove = new TurnAction();
        
        chosenMove.queenAction = new Wait("Wololoo");
        chosenMove.trainAction = new HaltTraing("Be patient"); 
        
        return chosenMove;
    }

    public void ParseInputs_Begin()
    {
        game = new GameInfo();
        string[] inputs;
        game.numSites = int.Parse(Console.ReadLine());
        
        for (int i = 0; i < game.numSites; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int siteId = int.Parse(inputs[0]);
            int x = int.Parse(inputs[1]);
            int y = int.Parse(inputs[2]);
            int radius = int.Parse(inputs[3]);
            
            SiteInfo newSiteInfo = new SiteInfo(siteId, new Position(x,y), radius);
            game.sites.Add(newSiteInfo);
        }
    }
}

public class Unit
{
    Position pos;
    int owner;
    int unitType;
    int health;

    public Unit(int x, int y, int owner, int unitType, int health)
    {
        this.pos = new Position(x,y);
        this.owner = owner;
        this.unitType = unitType;
        this.health = health;
    }
}

public class GameState
{
    public List<Site> sites = new List<Site>();
    public List<Unit> units = new List<Unit>();
    public int gold;
    public int touchedSite;

    public int numUnits => units.Count;
}

public class GameInfo
{
    public List<SiteInfo> sites = new List<SiteInfo>();
    public int numSites;

    public string GetSites_ToString()
    {
        return sites.Aggregate("", (agg, x) => agg + "\n"+ x.ToString());
    }
}

public class SiteInfo
{
    public int siteId;
    public Position pos;
    public int radius;

    public SiteInfo(int siteId, Position pos, int radius)
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

public class Site
{
    int siteId;
    int ignore1;
    int ignore2;
    int structureType;
    int owner;
    int param1;
    int param2;

    public Site(int siteId, int ignore1, int ignore2, int structureType, int owner, int param1, int param2)
    {
        this.siteId = siteId;
        this.ignore1 = ignore1;
        this.ignore2 = ignore2;
        this.structureType = structureType;
        this.owner = owner;
        this.param1 = param1;
        this.param2 = param2;
    }
}

#endregion        

class Player
{
    static void Main(string[] args)
    {
        LaPulzellaD_Orleans giovannaD_Arco = new LaPulzellaD_Orleans();

        giovannaD_Arco.ParseInputs_Begin();
        
        while (true)
        {
            giovannaD_Arco.ParseInputs_Turn();
            TurnAction move = giovannaD_Arco.think();
            move.PrintMove();
        }
    }
}