/** Code by Oran Bar **/

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/

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

    public double DistanceSqr(Position distanceTo)
    {
        return Math.Pow((this.x - distanceTo.x), 2) + Math.Pow((this.y - distanceTo.y), 2);
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

    public HaltTraing(string message = "") : base(message)
    {
        if(message != ""){throw new ArgumentException();}
    }

    
    public override string ToString_Impl()
    {
        return this.actionName;
    }
}

public class Train : IAction
{
    public string actionName = "TRAIN";
    public List<int> siteIds;

    public Train(string message = "") : base(message){}

    public Train(IEnumerable<Site> baraccksesToTrainFrom)
    {
        siteIds = baraccksesToTrainFrom.Select(s => s.siteId).ToList();
    }

    public override string ToString_Impl()
    {
        string result = this.actionName;
        foreach (var siteId in siteIds)
        {
            result += " " + siteId;
        }

        return result;
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

    public Move(Position pos, string message = "") : base(message)
    {
        targetPos = pos;
    }
    
    public Move(int x, int y, string message = "") : base(message)
    {
        targetPos = new Position(x,y);
    }
    
    public override string ToString_Impl()
    {
        return actionName + " " + targetPos.x +" " + targetPos.y;
    }
}

public class BuildBarracks : IAction
{
    public int siteId;
    public string barracks_type;

    public BuildBarracks(int siteId, BarracksType barracksType, string message = "") : base(message)
    {
        this.siteId = siteId;
        barracks_type = (barracksType == BarracksType.Knight) ? "KNIGHT" : "ARCHER" ;
    }

    public override string ToString_Impl()
    {
        return $"BUILD {siteId} BARRACKS-{barracks_type}";
    }
}

public class BuildMine : IAction
{
    public int siteId;

    public BuildMine(int siteId, string message = "") : base(message)
    {
        this.siteId = siteId;
    }

    public override string ToString_Impl()
    {
        return $"BUILD {siteId} MINE";
    }
}

public class BuildTower : IAction
{
    public int siteId;

    public BuildTower(int siteId, string message = "") : base(message)
    {
        this.siteId = siteId;
    }

    public override string ToString_Impl()
    {
        return $"BUILD {siteId} MINE";
    }
}

public enum BarracksType
{
    Knight = 0,
    Archer = 1
}

#endregion

#endregion


#region Game Structures

public class Unit
{
    public Position pos;
    public Owner owner;
    public UnitType unitType;
    public int health;

    public Unit(int x, int y, int owner, int unitType, int health)
    {
        this.pos = new Position(x,y);
        this.owner = (Owner) owner;
        this.unitType = (UnitType) unitType;
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

    public Unit MyQueen => units.First(u => u.owner == Owner.Friendly && u.unitType == UnitType.Queen);
}

public enum UnitType
{
    Queen = -1,
    Knight = 0,
    Archer = 1,
    Giant = 2
}

public enum StructureType
{
    None = -1,
    Mine = 0,
    Tower = 1,
    Barrcks = 2
}

public enum Owner
{
    Neutral = -1,
    Friendly = 0,
    Enemy = 1
}

public class GameInfo
{
    public Dictionary<int, SiteInfo> sites = new Dictionary<int, SiteInfo>();
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
    public int siteId;
    public int gold;
    public int maxMineSize;
    public StructureType structureType;
    public Owner owner;
    public int param1;
    public UnitType creepsType;

    public Site(int siteId, int gold, int maxMineSize, int structureType, int owner, int param1, int param2)
    {
        this.siteId = siteId;
        this.gold = gold;
        this.maxMineSize = maxMineSize;
        this.structureType = (StructureType) structureType;
        this.owner = (Owner) owner;
        this.param1 = param1;
        this.creepsType = (UnitType) param2;
    }
}

#endregion        


public class LaPulzellaD_Orleans
{
    public static int MAX_MINES = 2, MAX_BARRACKSES_KNIGHTS = 1, MAX_BARRACKSES_ARCER = 1;
    
    public GameInfo game;

    public GameState currGameState;
    
    public TurnAction think()
    {
        TurnAction chosenMove = new TurnAction();
        Unit myQueen = currGameState.MyQueen;

        //If we are touching a site, we do something with it

        IEnumerable<Site> mySites = currGameState.sites.Where(s => s.owner == Owner.Friendly);
        
        // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
        Site closestUnbuiltSite = SortSites_ByDistance(myQueen.pos, currGameState.sites)
            .Where(s => s.owner == Owner.Neutral)
            .FirstOrDefault();

        Site touchedSite = null;
        if (currGameState.touchedSite != -1)
        {
            touchedSite = currGameState.sites[currGameState.touchedSite];
        }

        
        int owned_knight_barrackses = mySites.Count(ob => ob.structureType == StructureType.Barrcks && ob.creepsType == UnitType.Knight);
        int owned_archer_barrackses = mySites.Count(ob => ob.structureType == StructureType.Barrcks && ob.creepsType == UnitType.Archer);
        int owned_giant_barrackses = mySites.Count(ob => ob.structureType == StructureType.Barrcks && ob.creepsType == UnitType.Giant);
        int owned_mines = mySites.Count(ob => ob.structureType == StructureType.Mine);
        
        int total_owner_barrackses = owned_archer_barrackses + owned_giant_barrackses + owned_knight_barrackses;
        
        bool touchingNeutralSite = touchedSite != null && touchedSite.owner == Owner.Neutral;
        bool touchingMyMine = touchedSite != null && touchedSite.owner == Owner.Friendly &&
                              touchedSite.structureType == StructureType.Mine;
        
        if (touchingNeutralSite)
        {
            //Build

            if (owned_mines < MAX_MINES)
            {
                chosenMove.queenAction = new BuildMine(currGameState.touchedSite);
            }
            else if (owned_knight_barrackses < MAX_BARRACKSES_KNIGHTS)
            {
                chosenMove.queenAction = new BuildBarracks(currGameState.touchedSite, BarracksType.Knight);
            }
            else if (owned_archer_barrackses < MAX_BARRACKSES_ARCER)
            {
                chosenMove.queenAction = new BuildBarracks(currGameState.touchedSite, BarracksType.Archer);
            }
        }
        else if (touchingMyMine && IsMineMaxed(touchedSite) == false)
        {
            chosenMove.queenAction = new BuildMine(currGameState.touchedSite);
        }
        else if(total_owner_barrackses < MAX_BARRACKSES_KNIGHTS + MAX_BARRACKSES_ARCER)
        {
            //Go to next closest site
            chosenMove.queenAction = new Move(GetSiteInfo(closestUnbuiltSite).pos);
        }
        else
        {
            //Run to angle
            Position[] angles = { new Position(0,0), new Position(1920,1000)};
            Position targetAngle;

            targetAngle = angles.OrderBy(a => myQueen.pos.DistanceSqr(a)).First();
            
            chosenMove.queenAction = new Move(targetAngle);
        }

        IEnumerable<Site> baraccksesToTrainFrom = mySites.Where(ob => ob.param1 == 0);
        
        if (baraccksesToTrainFrom.Any())
        {
            //Train
            chosenMove.trainAction = new Train(baraccksesToTrainFrom); 
        }
        else
        {
            //Wait
            chosenMove.trainAction = new HaltTraing();
        }
        
        return chosenMove;
    }

    public List<Site> SortSites_ByDistance(Position startPosition, List<Site> siteList)
    {
        IOrderedEnumerable<Site> sortedSitesStream = 
            siteList
                .OrderBy(s => startPosition.DistanceSqr(GetSiteInfo(s.siteId).pos));

        return sortedSitesStream.ToList();

    }

    private bool IsMineMaxed(Site site)
    {
        return site.param1 == site.maxMineSize;
    }

    public SiteInfo GetSiteInfo(Site site)
    {
        return GetSiteInfo(site.siteId);
    }
    
    public SiteInfo GetSiteInfo(int siteId)
    {
        return game.sites[siteId];
    }
    
    public void ParseInputs_Turn()
    {
        currGameState = new GameState();
        
        var inputs = Console.ReadLine().Split(' ');
        currGameState.gold = int.Parse(inputs[0]);
        currGameState.touchedSite = int.Parse(inputs[1]); // -1 if none
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
            currGameState.sites.Add(site);
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
            currGameState.units.Add(unit);
        }
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
            game.sites[siteId] = newSiteInfo;
        }
    }
}

