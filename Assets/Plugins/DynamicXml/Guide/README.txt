
Recommended parsing structure for xml files:
---------------------------------//
while (xmlReader.Read())
{
	switch(xmlReader.Name)
	{
		case "exampleElementName":
		if (xmlReader.ReadToDescendant("childElementName"))
		{
			do
			{
			
			} while (xmlReader.ReadToNextSibling("childElementName"));
		}
		break;
	}
}
---------------------------------//
