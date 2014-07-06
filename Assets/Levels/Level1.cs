public class Level1 : LevelSource
{
	public override void Generate()
	{
		m_GoldRate = 0.7f;
		Text("##     ##", 10);
		Text("### # ###", 20);
		Text("### O ###", 3);
		Text("##### ###", 7);
		Text("#### O###", 5);
		Text("####   ##", 3);
		Text("#### # ##", 5);
		Text("###  #O##", 5);
		Loop ();
		Text("###O##^##");
		Text("### ##O##", 5);
		Repeat(5);
		Text("##O #  ##", 5);
		Text("### ## ##", 10);
		Loop ();
		Text("##O^##^##");
		Text("## O##O##", 10);
		Repeat(5);
		Text("##   #O##");
		Text("## O#O ##", 30);
		Text("## O#<>##", 1);
		Text("##     ##", 10);
		Text("###   ###", 10);
		Text("#### ####", 30);
		Text("####>####");
		Text("#### ####", 20);
		Text("####<####");
		Text("#### ####", 20);
		Text("####^####");
		Text("#### ####", 20);
	}
}

