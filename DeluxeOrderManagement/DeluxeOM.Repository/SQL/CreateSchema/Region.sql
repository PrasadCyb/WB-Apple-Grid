
CREATE Table Region
(
	Id	INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	NAME NVARCHAR(255) NOT NULL 
)

--Populate Region
INSERT INTO REGION Select DISTINCT Region FROM TerritoryMapping_Staging
