using Minecraft.Server.FourKit.Entity;

namespace Respublica;

public class MCPlr
{
    public string name { get; set; } = "";
    public Guid uid { get; set; } = Guid.Empty;
}

public class DBPlr : MCPlr
{
    public LiteDB.ObjectId id { get; set; } = LiteDB.ObjectId.NewObjectId();
}
// UNI - literally only made this for guid to username LOL
public static class Plr
{
    public static string guidToUsrname(Guid uid)
    {
        var col = Database.Instance.GetCollection<DBPlr>("plr");

        var plr = col.Find(LiteDB.Query.EQ("uid", uid)).FirstOrDefault();

        if (plr == null)
        {
            Console.WriteLine("[RESPUBLICA] No GUID match for guidToUsrname!");
            return string.Empty;
        }
        return plr.name;
    }

    public static Guid usrToGuid(string name)
    {
        var col = Database.Instance.GetCollection<DBPlr>("plr");

        var plr = col.Find(LiteDB.Query.EQ("name", name)).FirstOrDefault();

        if (plr == null)
        {
            Console.WriteLine("[RESPUBLICA] No GUID match for guidToUsrname!");
            return Guid.Empty;
        }
        return plr.uid;
    }
}

public static partial class DBInteract
{
    public static void initPlr(Guid uid, string name)
    {
        var col = Database.Instance.GetCollection<DBPlr>("plr");

        if (col.Exists(x => x == new MCPlr { name = name, uid = uid }))
        {
            Console.WriteLine("[RESPUBLICA] User already exists!");
            return;
        }

        var plr = new DBPlr
        {
            name = name,
            uid = uid
        };

        col.Insert(plr);
    }
    public static void initPlr(Player plr)
    {
        var col = Database.Instance.GetCollection<DBPlr>("plr");

        if (col.Exists(x => x == new MCPlr { name = plr.getName(), uid = plr.getUniqueId() }))
        {
            Console.WriteLine("[RESPUBLICA] User already exists!");
            return;
        }

        var dbplr = new DBPlr
        {
            name = plr.getName(),
            uid = plr.getUniqueId()
        };

        col.Insert(dbplr);
    }
    public static void updatePlr(Guid uid, string name)
    {
        var col = Database.Instance.GetCollection<DBPlr>("plr");

        if (!col.Exists(LiteDB.Query.EQ("uid", uid)))
        {
            Console.WriteLine("[RESPUBLICA] User doesn't exist!");
            return;
        }

        var newplr = col.Find(LiteDB.Query.EQ("uid", uid)).First();
        newplr.name = name;

        col.Update(newplr);
    }
    public static bool isPlrReal(Guid uid) => Database.Instance.GetCollection<DBPlr>("plr").Exists(LiteDB.Query.EQ("uid", uid));
}