namespace Respublica;

public class Nation {
	public string name { get; set; } = "";
	public LiteDB.ObjectId capital { get; set; } = LiteDB.ObjectId.Empty;
	public Guid king { get; set; } = Guid.Empty;
}

public class DBNation : Nation {
	public LiteDB.ObjectId id { get; set; } = LiteDB.ObjectId.NewObjectId(); // if wrong func name fix
}
