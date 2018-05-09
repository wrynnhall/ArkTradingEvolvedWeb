/* Check if database already exists and delete it if it does exist*/
IF EXISTS(SELECT 1 FROM master.dbo.sysdatabases WHERE name = 'tradingevolved')
BEGIN
	DROP DATABASE tradingevolved
	print '' print '*** dropping database tradingevolved'
END
GO

print '' print '*** creating database tradingevolved'
GO
CREATE DATABASE tradingevolved
GO

print '' print '*** using database tradingevolved'
GO
USE [tradingevolved]
GO

/* *** Adding all tables *** */

print '' print '*** Creating User Table'
GO
CREATE TABLE [dbo].[User](
	[UserID]		[int] IDENTITY(100000,1)	NOT NULL,
	[Gamertag]		[nvarchar](30)		NOT NULL,
	[FirstName]		[nvarchar](50)		NOT NULL,
	[LastName]		[nvarchar](100)		NOT NULL,
	[PhoneNumber]	[nvarchar](15)		NULL,
	[Email]			[nvarchar](100)		NOT NULL,
	[PasswordHash]	[nvarchar](100)		NOT NULL DEFAULT 'b03ddf3ca2e714a6548e7495e2a03f5e824eaac9837cd7f159c67b90fb4b7342', /* Default password is 'P@ssw0rd' */
	[Active]		[bit]				NOT NULL DEFAULT 1,
	CONSTRAINT [pk_UserID] PRIMARY KEY([UserID] ASC),
	CONSTRAINT [ak_Email] UNIQUE ([Email] ASC)
)
GO

-- 9c9064c59f1ffa2e174ee754d2979be80dd30db552ec03e7e327e9b1a4bd594e
print '' print '*** Creating Role Table'
GO
CREATE TABLE [dbo].[Role](
	[RoleID]	[nvarchar](30)	NOT NULL,
	CONSTRAINT [pk_RoleID] PRIMARY KEY([RoleID] ASC)
)
GO


print '' print '*** Creating UserRole Table'
GO
CREATE TABLE [dbo].[UserRole](
	[UserID]	[int] 			NOT NULL,
	[RoleID]	[nvarchar](30)	NOT NULL,
	[Active]	[bit]			NOT NULL DEFAULT 1,
	CONSTRAINT [pk_User_UserID_Role_RoleID] PRIMARY KEY([RoleID], [UserID] ASC)
)


print '' print '*** Creating Creature Table'
GO
CREATE TABLE [dbo].[Creature](
	[CreatureID]	[nvarchar](50)	NOT NULL,
	[CreatureTypeID]	[nvarchar](30)	NOT NULL,
	[CreatureDietID]	[nvarchar](30)	NOT NULL,
	[Active]			[bit]			NOT NULL DEFAULT 1,
	CONSTRAINT [pk_CreatureID] PRIMARY KEY([CreatureID] ASC)
)
GO


print '' print '*** Creating CreatureType Table'
GO
CREATE TABLE [dbo].[CreatureType](
	[CreatureTypeID]	[nvarchar](30)	NOT NULL,
	[Active]			[bit]			NOT NULL DEFAULT 1,
	CONSTRAINT [pk_CreatureTypeID] PRIMARY KEY([CreatureTypeID] ASC)
)
GO


print '' print '*** Creating CreatureDiet Table'
GO
CREATE TABLE [dbo].[CreatureDiet](
	[CreatureDietID]	[nvarchar](30)	NOT NULL,
	[Active]			[bit]			NOT NULL DEFAULT 1,
	CONSTRAINT [pk_CreatureDietID] PRIMARY KEY([CreatureDietID] ASC)
)
GO


print '' print '*** Creating Collection Table'
GO
CREATE TABLE [dbo].[Collection](
	[CollectionID]	[int] IDENTITY(200000,1)	NOT NULL,
	[UserID]		[int]				NOT NULL,
	[Name]			[nvarchar](100)		NOT NULL,
	[Active]		[bit]				NOT NULL DEFAULT 1,
	CONSTRAINT [pk_CollectionID] PRIMARY KEY([CollectionID] ASC)
)
GO


print '' print '*** Creating CollectionEntry Table'
GO
CREATE TABLE [dbo].[CollectionEntry](
	[CollectionEntryID]	[int] IDENTITY(300000,1)	NOT NULL,
	[CollectionID]		[int]			NOT NULL,
	[CreatureID]		[nvarchar](50)	NOT NULL,
	[Name]				[nvarchar](50)	NOT NULL,
	[Level]				[int]			NOT NULL,
	[Health]			[int]			NULL,
	[Stamina]			[int]			NULL,
	[Oxygen]			[int]			NULL,
	[Food]				[int]			NULL,
	[Weight]			[int]			NULL,
	[BaseDamage]		[int]			NULL,
	[MovementSpeed]		[int]			NULL,
	[Torpor]			[int]			NULL,
	[Imprint]			[int]			NULL,
	[Active]			[bit]			NOT NULL DEFAULT 1,
	CONSTRAINT [pk_CollectionEntryID] PRIMARY KEY([CollectionEntryID] ASC)
)
GO


print '' print '*** Creating MarketEntry Table'
GO
CREATE TABLE [dbo].[MarketEntry](
	[MarketEntryID]	[int] IDENTITY(400000,1)	NOT NULL,
	[CollectionEntryID]		[int]			NOT NULL,
	[MarketEntryStatusID]	[nvarchar](30)	NOT NULL,
	[ResourceID]			[nvarchar](30)	NOT NULL,
	[Units]					[int]			NOT NULL,
	[Active]				[bit]			NOT NULL DEFAULT 1,
	CONSTRAINT [pk_MarketEntryID] PRIMARY KEY([MarketEntryID] ASC)
)
GO


print '' print '*** Creating MarketEntryStatus Table'
GO
CREATE TABLE [dbo].[MarketEntryStatus](
	[MarketEntryStatusID]	[nvarchar](30)	NOT NULL,
	CONSTRAINT [pk_MarketEntryStatusID] PRIMARY KEY([MarketEntryStatusID] ASC)
)
GO


print '' print '*** Creating MarketEntryPurchase Table'
GO
CREATE TABLE [dbo].[MarketEntryPurchase](
	[UserID]			[int]		NOT NULL,
	[MarketEntryID]		[int]		NOT NULL,
	CONSTRAINT [pk_User_UserID_MarketEntry_MarketEntryID] PRIMARY KEY([UserID], [MarketEntryID] ASC)
)
GO


print '' print '*** Creating Message Table'
GO
CREATE TABLE [dbo].[Message](
	[MessageID]	[int]	IDENTITY(500000,1)	NOT NULL,
	[UserID]		[int]				NOT NULL,
	[MarketEntryID]	[int]				NOT NULL,
	[Text]			[nvarchar](150)		NOT NULL,
	[SentTime]		[datetime]			NOT NULL,
	CONSTRAINT [pk_MessageID] PRIMARY KEY([MessageID] ASC)
)
GO


print '' print '*** Creating Resource Table'
GO
CREATE TABLE [dbo].[Resource](
	[ResourceID]	[nvarchar](30)	NOT NULL,
	[Active]		[bit]	DEFAULT 1 NOT NULL,
	CONSTRAINT [pk_ResourceID] PRIMARY KEY([ResourceID] ASC)
)
GO


/* *** Adding constraints *** */


print '' print '*** Creating UserRole Constraints'
GO
ALTER TABLE [dbo].[UserRole] WITH NOCHECK
	ADD CONSTRAINT [fk_UserRole_UserID]
	FOREIGN KEY([UserID])
	REFERENCES [dbo].[User]([UserID])
	ON UPDATE CASCADE,
	CONSTRAINT [fk_UserRole_RoleID]
	FOREIGN KEY([RoleID])
	REFERENCES [dbo].[Role]([RoleID])
	ON UPDATE CASCADE
GO


print '' print '*** Creating Creature Constraints'
GO
ALTER TABLE [dbo].[Creature] WITH NOCHECK
	ADD CONSTRAINT [fk_CreatureType_Creature] 
	FOREIGN KEY([CreatureTypeID]) 
	REFERENCES [dbo].[CreatureType]([CreatureTypeID]) 
	ON UPDATE CASCADE,
	CONSTRAINT [fk_CreatureDiet_Creature]
	FOREIGN KEY([CreatureDietID])
	REFERENCES [dbo].[CreatureDiet]([CreatureDietID])
	ON UPDATE CASCADE
GO





print '' print '*** Creating Collection Constraints'
GO
ALTER TABLE [dbo].[Collection]
	ADD CONSTRAINT [fk_User_Collection]
	FOREIGN KEY([UserID])
	REFERENCES [dbo].[User]([UserID])
	ON UPDATE CASCADE
GO


print '' print '*** Creating CollectionEntry Constraints'
GO
ALTER TABLE [dbo].[CollectionEntry]
	ADD CONSTRAINT [fk_Collection_CollectionEntry]
	FOREIGN KEY([CollectionID])
	REFERENCES [dbo].[Collection]([CollectionID])
	ON UPDATE CASCADE,
	CONSTRAINT [fk_Creature_CollectionEntry]
	FOREIGN KEY([CreatureID])
	REFERENCES [dbo].[Creature]([CreatureID])
	ON UPDATE CASCADE
GO


print '' print '*** Creating MarketEntry Constraints'
GO
ALTER TABLE [dbo].[MarketEntry]
	ADD CONSTRAINT [fk_CollectionEntry_MarketEntry]
	FOREIGN KEY([CollectionEntryID])
	REFERENCES [dbo].[CollectionEntry]([CollectionEntryID])
	ON UPDATE CASCADE,
	CONSTRAINT [fk_MarketEntryStatus_MarketEntry]
	FOREIGN KEY([MarketEntryStatusID])
	REFERENCES [dbo].[MarketEntryStatus]([MarketEntryStatusID])
	ON UPDATE CASCADE,
	CONSTRAINT [fk_Resource_MarketEntry]
	FOREIGN KEY([ResourceID])
	REFERENCES [dbo].[Resource]([ResourceID])
	ON UPDATE CASCADE
GO


print '' print '*** Creating MarketEntryPurchase Constraints'
GO
ALTER TABLE [dbo].[MarketEntryPurchase]
	ADD CONSTRAINT [fk_MarketEntry_MarketEntryPurchase]
	FOREIGN KEY([MarketEntryID])
	REFERENCES [dbo].[MarketEntry]([MarketEntryID])
	ON UPDATE CASCADE,
	CONSTRAINT [fk_User_MarketEntryPurchase]
	FOREIGN KEY([UserID])
	REFERENCES [dbo].[User]([UserID])
	ON UPDATE NO ACTION
GO


print '' print '*** Creating Message Constraints'
GO
ALTER TABLE [dbo].[Message]
	ADD CONSTRAINT [fk_User_Message]
	FOREIGN KEY([UserID])
	REFERENCES [dbo].[User]([UserID])
	ON UPDATE CASCADE,
	CONSTRAINT [fk_MarketEntry_Message]
	FOREIGN KEY([MarketEntryID])
	REFERENCES [dbo].[MarketEntry]([MarketEntryID])
GO

	
/* *** Adding insert data *** */

print '' print '*** Creating Role Inserts'
GO
INSERT INTO [dbo].[Role]
		([RoleID])
	VALUES
		('Admin'),
		('General')
GO



print '' print '*** Creating User Inserts'
GO
INSERT INTO [dbo].[User]
		([Gamertag], [FirstName], [LastName], [PhoneNumber], [Email])
	VALUES
		('CaptainProdigy', 'Zachary', 'Hall', '3191234567', 'captainprodigy@gmail.com'),
		('BahamutUnknown', 'Travis', 'Johnson', '4353244321', 'dinotravman123@gmail.com'),
		('ColtLuger421', 'Colten', 'Burdik', '7689032156', 'admin@ark.com')
GO


print '' print '*** Creating UserRole Inserts'
GO
INSERT INTO [dbo].[UserRole]
		([UserID], [RoleID])
	VALUES
		(100001, 'General'),
		(100000, 'General'),
		(100002, 'Admin')
GO


print '' print '*** Creating CreatureType Inserts'
GO
INSERT INTO [dbo].[CreatureType]
		([CreatureTypeID])
	VALUES
		('Amphibian'),
		('Bird'),
		('Dinosaur'),
		('Fish'),
		('Invertebrate'),
		('Mammal'),
		('Reptile'),
		('Synapsid'),
		('Fantasy')
GO


print '' print '*** Creating CreatureDiet Inserts'
GO
INSERT INTO [dbo].[CreatureDiet]
		([CreatureDietID])
	VALUES
		('Carnivore'),
		('Herbivore'),
		('Omnivore'),
		('Piscivore'),
		('Carrion'),
		('Coprophagic'),
		('Flame'),
		('Mineral'),
		('Sanguinivore')
GO


print '' print '*** Creating Creature Inserts'
GO
INSERT INTO [dbo].[Creature]
		([CreatureID], [CreatureTypeID], [CreatureDietID])
	VALUES
		('Achatina', 'Invertebrate', 'Herbivore'),
		('Allosaurus', 'Dinosaur', 'Carnivore'),
		('Anglerfish', 'Fish', 'Carnivore'),
		('Ankylosaurus', 'Dinosaur', 'Herbivore'),
		('Araneo', 'Invertebrate', 'Carnivore'),
		('Archaeopteryx', 'Bird', 'Carnivore'),
		('Argentavis', 'Bird', 'Carrion'),
		('Arthropluera', 'Invertebrate', 'Carrion'),
		('Baryonyx', 'Dinosaur', 'Piscivore'),
		('Basilosaurus', 'Mammal', 'Piscivore'),
		('Beelzebufo', 'Amphibian', 'Carnivore'),
		('Brontosaurus', 'Dinosaur', 'Herbivore'),
		('Carbonemys', 'Reptile', 'Herbivore'),
		('Carnotaurus', 'Dinosaur', 'Carnivore'),
		('Castoroides', 'Mammal', 'Herbivore'),
		('Chalicotherium', 'Mammal', 'Herbivore'),
		('Compy', 'Dinosaur', 'Carnivore'),
		('Daeodon', 'Mammal', 'Carnivore'),
		('Dilophosaur', 'Dinosaur', 'Carnivore'),
		('Dimetrodon', 'Synapsid', 'Carnivore'),
		('Dimorphodon', 'Reptile', 'Carnivore'),
		('Diplocaulus', 'Amphibian', 'Piscivore'),
		('Diplodocus', 'Dinosaur', 'Herbivore'),
		('Direbear', 'Mammal', 'Omnivore'),
		('Direwolf', 'Mammal', 'Carnivore'),
		('Dodo', 'Bird', 'Herbivore'),
		('Doedicurus', 'Mammal', 'Herbivore'),
		('Dung Beetle', 'Invertebrate', 'Coprophagic'),
		('Dunkleosteus', 'Fish', 'Carnivore'),
		('Electrophorus', 'Fish', 'Carnivore'),
		('Equus', 'Mammal', 'Herbivore'),
		('Gallimimus', 'Dinosaur', 'Herbivore'),
		('Giant Bee', 'Invertebrate', 'Herbivore'),
		('Giganotosaurus', 'Dinosaur', 'Carnivore'),
		('Gigantopithecus', 'Mammal', 'Herbivore'),
		('Griffin', 'Fantasy', 'Carnivore'),
		('Hesperornis', 'Bird', 'Piscivore'),
		('Hyaenodon', 'Mammal', 'Carnivore'),
		('Ichthy', 'Reptile', 'Carnivore'),
		('Ichthyornis', 'Bird', 'Piscivore'),
		('Iguanodon', 'Dinosaur', 'Herbivore'),
		('Jerboa', 'Mammal', 'Herbivore'),
		('Kairuku', 'Bird', 'Piscivore'),
		('Kaprosuchus', 'Reptile', 'Carnivore'),
		('Kentrosaurus', 'Dinosaur', 'Herbivore'),
		('Leech', 'Invertebrate', 'Sanguinivore'),
		('Liopleurodon', 'Reptile', 'Carnivore'),
		('Lymantria', 'Invertebrate', 'Herbivore'),
		('Lystrosaurus', 'Synapsid', 'Herbivore'),
		('Mammoth', 'Mammal', 'Herbivore'),
		('Manta', 'Fish', 'Carnivore'),
		('Mantis', 'Invertebrate', 'Carnivore'),
		('Megalania', 'Reptile', 'Carnivore'),
		('Megaloceros', 'Mammal', 'Herbivore'),
		('Megalodon', 'Fish', 'Carnivore'),
		('Megalosaurus', 'Dinosaur', 'Carnivore'),
		('Megatherium', 'Mammal', 'Omnivore'),
		('Mesopithecus', 'Mammal', 'Omnivore'),
		('Microraptor', 'Dinosaur', 'Carnivore'),
		('Morellatops', 'Dinosaur', 'Herbivore'),
		('Mosasaurus', 'Reptile', 'Carnivore'),
		('Moschops', 'Synapsid', 'Omnivore'),
		('Onyc', 'Mammal', 'Omnivore'),
		('Otter', 'Mammal', 'Omnivore'),
		('Oviraptor', 'Dinosaur', 'Carnivore'),
		('Ovis', 'Mammal', 'Herbivore'),
		('Pachy', 'Dinosaur', 'Herbivore'),
		('Pachyrhinosaurus', 'Dinosaur', 'Herbivore'),
		('Paracer', 'Mammal', 'Herbivore'),
		('Parasaur', 'Dinosaur', 'Herbivore'),
		('Pegomastax', 'Dinosaur', 'Herbivore'),
		('Pelagornis', 'Bird', 'Piscivore'),
		('Phiomia', 'Mammal', 'Herbivore'),
		('Phoenix', 'Fantasy', 'Flame'),
		('Plesiosaur', 'Reptile', 'Carnivore'),
		('Procoptodon', 'Mammal', 'Herbivore'),
		('Pteranodon', 'Reptile', 'Carnivore'),
		('Pulmonoscorpius', 'Invertebrate', 'Carnivore'),
		('Purlovia', 'Synapsid', 'Carnivore'),
		('Quetzal', 'Reptile', 'Carnivore'),
		('Raptor', 'Dinosaur', 'Carnivore'),
		('Rex', 'Dinosaur', 'Carnivore'),
		('Rock Elemental', 'Fantasy', 'Mineral'),
		('Sabertooth', 'Mammal', 'Carnivore'),
		('Sarco', 'Reptile', 'Carnivore'),
		('Spinosaur', 'Dinosaur', 'Carnivore'),
		('Stegosaurus', 'Dinosaur', 'Herbivore'),
		('Tapejara', 'Reptile', 'Carnivore'),
		('Terror Bird', 'Bird', 'Carnivore'),
		('Therizinosaur', 'Dinosaur', 'Herbivore'),
		('Thorny Dragon', 'Reptile', 'Carnivore'),
		('Thylacoleo', 'Mammal', 'Carnivore'),
		('Titanoboa', 'Mammal', 'Carnivore'),
		('Triceratops', 'Dinosaur', 'Herbivore'),
		('Troodon', 'Dinosaur', 'Carnivore'),
		('Tusoteuthis', 'Invertebrate', 'Carnivore'),
		('Vulture', 'Bird', 'Carrion'),
		('Woolly Rhino', 'Mammal', 'Herbivore'),
		('Wyvern', 'Fantasy', 'Carnivore'),
		('Yutyrannus', 'Dinosaur', 'Carnivore')
GO



print '' print '*** Creating Collection Inserts'
GO
INSERT INTO [dbo].[Collection]
		([UserID], [Name])
	VALUES
		(100001, 'Island 54'),
		(100001, 'Center 32'),
		(100002, 'Birds'),
		(100002, 'Dinosaurs'),
		(100000, 'Ragnarok'),
		(100000, 'Island54')
GO


print '' print '*** Creating CollectionEntry Inserts'
GO
INSERT INTO [dbo].[CollectionEntry]
		([CollectionID], [CreatureID], [Name], [Level], [Health], [Stamina], [Oxygen]
		, [Food], [Weight], [BaseDamage], [MovementSpeed], [Torpor], [Imprint])
	VALUES
		(200000, 'Pteranodon', 'Ole Faithful', 156, 356, 1200, 250, 150000, 975, 150, 100, 33000, 95),
		(200000, 'Rex', 'Big Guy', 25000, 356, 1200, 250, 150000, 975, 150, 100, 33000, 95),
		(200000, 'Quetzal', 'Donny', 35000, 356, 1200, 250, 150000, 975, 150, 100, 33000, 95),
		(200000, 'Wyvern', 'Saphira', 658, 1890, 1200, 250, 150000, 975, 150, 100, 33000, 95),
		(200001, 'Mantis', 'Jackie', 1200, 1890, 1200, 250, 150000, 975, 150, 100, 33000, 95),
		(200001, 'Griffin', 'Peter', 13740, 1890, 1200, 250, 150000, 975, 150, 100, 33000, 95),
		(200001, 'Ankylosaurus', 'Rocky', 23480, 1890, 1200, 250, 150000, 975, 150, 100, 33000, 95),
		(200001, 'Triceratops', 'StabbyMcStabbyFace', 13678, 1890, 1200, 250, 150000, 975, 150, 100, 33000, 95),
		(200004, 'Argentavis', 'Pidgey', 256, 15000, 1420, 2500, 52000, 800, 125, 100, 18520, 19),
		(200004, 'Tusoteuthis', 'Squidward', 240, 12362, 56820, 80000, 25069, 1203, 524, 195, 5489, 195),
		(200004, 'Baryonyx', 'Barry', 145, 1200, 569, 512, 616, 561, 165, 12356, 156, 954),
		(200005, 'Chalicotherium', 'Clobberer', 2156, 2514, 2312, 1651, 5651, 250, 156, 198, 25466, 123)
		
GO


print '' print '*** Creating Resource Inserts'
GO
INSERT INTO [dbo].[Resource]
		([ResourceID])
	VALUES
		('Metal Behemoth Gate'),
		('Metal Ingot'),
		('Silica Pearl'),
		('Turret')
GO


print '' print '*** Creating MarketEntryStatus Inserts'
GO
INSERT INTO [dbo].[MarketEntryStatus]
		([MarketEntryStatusID])
	VALUES
		('Available'),
		('Sold'),
		('Closed'),
		('Complete')
GO


print '' print '*** Creating MarketEntry Inserts'
GO
INSERT INTO [dbo].[MarketEntry]
		([CollectionEntryID], [MarketEntryStatusID], [ResourceID], [Units])
	VALUES
		(300000, 'Sold', 'Metal Ingot', 1000),
		(300002, 'Available', 'Metal Behemoth Gate', 15),
		(300004, 'Available', 'Silica Pearl', 25000),
		(300003, 'Available', 'Turret', 1000),
		(300010, 'Available', 'Metal Behemoth Gate', 10),
		(300009, 'Sold', 'Silica Pearl', 450),
		(300011, 'Available', 'Silica Pearl', 250)
GO

print '' print '*** Creating MarketEntry Inserts'
GO
INSERT INTO [dbo].[MarketEntryPurchase]
		([UserID], [MarketEntryID])
	VALUES
		(100000, 400000),
		(100001, 400005)
GO


print '' print '*** Creating Message Inserts'
GO
INSERT INTO [dbo].[Message]
		([UserID], [MarketEntryID], [Text], [SentTime])
	VALUES
		(100000, 400000, 'Hey, I think your ptera is cool. I''m willing to pay the metal ingots for it.', '2017-12-12 22:44:31'),
		(100001, 400000, 'cool', '2017-12-12 23:02:21'),
		(100001, 400000, 'when would you like to make the trade? Im'' available most nights from 6-9 and on the weekend I''ll probably be on all day because  I have no life', '2017-12-12 23:03:31'),
		(100001, 400005, 'Yo, give me dat squid', '2017-12-12 23:04:44'),
		(100000, 400005, 'k, but u got the stuff for it rite??', '2017-12-12 23:05:39'),
		(100000, 400000, 'How about tomorrow at 8? What is your server?', '2017-12-12 23:06:25')
GO


/* *** Adding functions *** */
print '' print '*** Creating fn_get_market_entry_active'
GO
CREATE FUNCTION [dbo].[fn_get_market_entry_active]
	(
	@MarketEntryStatusID [nvarchar](30)
	)
	RETURNS [bit]
AS
	BEGIN
		DECLARE @active [bit]
		
		IF (@MarketEntryStatusID IN ('Closed', 'Complete'))
			BEGIN 
				SET @active = 0
			END
		ELSE
			BEGIN
				SET @active = 1
			END
	
	RETURN @active
	END
GO


/* *** Adding stored procedures *** */


print '' print '*** Creating sp_authenticate_user'
GO
CREATE PROCEDURE [dbo].[sp_authenticate_user]
	(
	@Email			[nvarchar](100),
	@PasswordHash	[nvarchar](100)
	)
AS
	BEGIN
		SELECT COUNT([UserID])
		FROM [User]
		WHERE [Email] = @Email
		AND [PasswordHash] = @PasswordHash
		AND [Active] = 1
	END
GO


print '' print '*** Creating sp_retrieve_user_roles'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_user_roles]
	(
	@UserID		[int]
	)
AS
	BEGIN
		SELECT [RoleID]
		FROM [UserRole]
		WHERE [UserRole].[UserID] = @UserID
		AND [Active] = 1
	END
GO


print '' print '*** Creating sp_update_passwordHash'
GO
CREATE PROCEDURE [dbo].[sp_update_passwordHash]
	(
	@UserID				[int],
	@OldPasswordHash	[nvarchar](100),
	@NewPasswordHash	[nvarchar](100)
	)
AS
	BEGIN
		UPDATE [User]
			SET [PasswordHash] = @NewPasswordHash
			WHERE [UserID] = @UserID
			AND [PasswordHash] = @OldPasswordHash
		RETURN @@ROWCOUNT
	END
GO


print '' print '*** Creating sp_retrieve_user_by_email'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_user_by_email]
	(
	@Email [nvarchar](100)
	)
AS
	BEGIN
		SELECT [UserID], [Gamertag], [FirstName], [LastName], [PhoneNumber], [Email], [Active]
		FROM [User]
		WHERE [Email] = @Email
	END
GO


print '' print '*** Creating sp_select_collection_entries_by_active'
GO
CREATE PROCEDURE [dbo].[sp_select_collection_entries_by_active]
	(
	@CollectionID	[int],
	@Active			[bit]
	)
AS
	BEGIN
		SELECT [CollectionEntryID], [CollectionID], [CreatureID], [Name], [Level], 
				[Health], [Stamina], [Oxygen], [Food], [Weight], [BaseDamage], 
				[MovementSpeed], [Torpor], [Imprint], [Active]
		FROM [CollectionEntry]
		WHERE [CollectionID] = @CollectionID
		AND [Active] = @Active
	END
GO


print '' print '*** Creating sp_select_collections_by_active'
GO
CREATE PROCEDURE [dbo].[sp_select_collections_by_active]
	(
	@UserID	[int],
	@Active [bit]
	)
AS
	BEGIN
		SELECT [CollectionID], [UserID], [Name], [Active]
		FROM [Collection]
		WHERE [UserID] = @UserID
		AND [Active] = @Active
	END
GO


print '' print '*** Creating sp_create_collection_entry'
GO
CREATE PROCEDURE [dbo].[sp_create_collection_entry]
	(
	@CollectionID		[int],			
	@CreatureID		[nvarchar](50),	
	@Name			[nvarchar](50),
	@Level			[int],			
	@Health			[int],			
	@Stamina		[int],			
	@Oxygen			[int],			
	@Food			[int],			
	@Weight			[int],			
	@BaseDamage		[int],			
	@MovementSpeed	[int],			
	@Torpor			[int],			
	@Imprint		[int],			
	@Active			[bit]
	)
AS
	BEGIN
		INSERT INTO [dbo].[CollectionEntry]
				(
				[CollectionID],
				[CreatureID],
				[Name],
				[Level],
				[Health],
				[Stamina],
				[Oxygen],
				[Food],
				[Weight],
				[BaseDamage],
				[MovementSpeed],
				[Torpor],
				[Imprint],
				[Active]
				)
		VALUES(
				@CollectionID,			
				@CreatureID,	
				@Name,
				@Level,			
				@Health,			
				@Stamina,			
				@Oxygen,			
				@Food,			
				@Weight,			
				@BaseDamage,			
				@MovementSpeed,			
				@Torpor,			
				@Imprint,			
				@Active
			)
	END
GO


print '' print '*** Creating sp_create_collection'
GO
CREATE PROCEDURE [dbo].[sp_create_collection]
	(
	@UserID		[int],			
	@Name		[nvarchar](100)
	)
AS
	BEGIN
		INSERT INTO [dbo].[Collection]
			(
			[UserID], [Name]
			)
		VALUES
			(@UserID, @Name)
	END
GO


print '' print '*** Creating sp_update_collection'
GO
CREATE PROCEDURE [dbo].[sp_update_collection]
	(
	@CollectionID	[int],
	@OldName		[nvarchar](100),
	@NewName		[nvarchar](100)
	)
AS
	BEGIN
		UPDATE [Collection]
		SET [Name] = @NewName
		WHERE [Name] = @OldName
		AND [CollectionID] = @CollectionID
		RETURN @@ROWCOUNT
	END
GO


print '' print '*** Creating sp_update_collection_active'
GO
CREATE PROCEDURE [dbo].[sp_update_collection_active]
	(
	@CollectionID	[int],
	@Active			[bit]
	)
AS
	BEGIN
		UPDATE [Collection]
		SET [Active] = @Active
		WHERE [CollectionID] = @CollectionID
		RETURN @@ROWCOUNT
	END
GO




print '' print '*** Creating sp_select_creature_by_id'
GO
CREATE PROCEDURE [dbo].[sp_select_creature_by_id]
	(
	@CreatureID		[nvarchar](50)
	)
AS
	BEGIN
		SELECT [CreatureID], [CreatureTypeID], [CreatureDietID], [Active]
		FROM [Creature]
		WHERE [CreatureID] = @CreatureID
	END
GO


print '' print '*** Creating sp_select_creatures'
GO
CREATE PROCEDURE [dbo].[sp_select_creatures]
	
AS
	BEGIN
		SELECT [CreatureID], [CreatureTypeID], [CreatureDietID]
		FROM [Creature]
		WHERE [Active] = 1
	END
GO


print '' print '*** Creating sp_update_collection_entry'
GO
CREATE PROCEDURE [dbo].[sp_update_collection_entry]
	(
	@CollectionEntryID	[int],
	@OldCreatureID		[nvarchar](50),
	@NewCreatureID		[nvarchar](50),
	@OldName			[nvarchar](50),
	@NewName			[nvarchar](50),
	@OldLevel			[int],
	@NewLevel			[int],
	@OldHealth			[int],
	@NewHealth			[int],
	@OldStamina			[int],
	@NewStamina			[int],
	@OldOxygen			[int],
	@NewOxygen			[int],
	@OldFood			[int],
	@NewFood			[int],
	@OldWeight			[int],
	@NewWeight			[int],
	@OldBaseDamage		[int],
	@NewBaseDamage		[int],
	@OldMovementSpeed	[int],
	@NewMovementSpeed	[int],
	@OldTorpor			[int],
	@NewTorpor			[int],
	@OldImprint			[int],
	@NewImprint			[int]
	)
AS
	BEGIN
		UPDATE [CollectionEntry]
		SET [CreatureID] = @NewCreatureID,
			[Name] = @NewName,
			[Level] = @NewLevel,
			[Health] = @NewHealth,
			[Stamina] = @NewStamina,
			[Oxygen] = @NewOxygen,
			[Food] = @NewFood,
			[Weight] = @NewWeight,
			[BaseDamage] = @NewBaseDamage,
			[MovementSpeed] = @NewMovementSpeed,
			[Torpor] = @NewTorpor,
			[Imprint] = @NewImprint
		WHERE [CollectionEntryID] = @CollectionEntryID
			AND [CreatureID] = @OldCreatureID
			AND [Name] = @OldName
			AND [Level] = @OldLevel
			AND [Health] = @OldHealth
			AND [Stamina] = @OldStamina
			AND [Oxygen] = @OldOxygen
			AND [Food] = @OldFood
			AND [Weight] = @OldWeight
			AND [BaseDamage] = @OldBaseDamage
			AND [MovementSpeed] = @OldMovementSpeed
			AND [Torpor] = @OldTorpor
			AND [Imprint] = @OldImprint
		RETURN @@ROWCOUNT
	END
GO


print '' print '*** Creating sp_select_market_entries_by_status'
GO
CREATE PROCEDURE [dbo].[sp_select_market_entries_by_status]
	(
	@MarketEntryStatusID	[nvarchar](30)
	)
AS
	BEGIN
		SELECT [MarketEntryID], [CollectionEntryID], [MarketEntryStatusID], [ResourceID], [Units], [Active]
		FROM [MarketEntry]
		WHERE [Active] = 1 AND [MarketEntryStatusID] = @MarketEntryStatusID
	END
GO


print '' print '*** Creating sp_select_market_entries_by_user'
GO
CREATE PROCEDURE [dbo].[sp_select_market_entries_by_user]
	(
	@UserID  [int]
	)
AS
	BEGIN
		SELECT [MarketEntry].[MarketEntryID], [MarketEntry].[CollectionEntryID], [MarketEntry].[MarketEntryStatusID], [MarketEntry].[ResourceID], [MarketEntry].[Units], [MarketEntry].[Active]
		FROM [MarketEntry]
		INNER JOIN [CollectionEntry] ON [MarketEntry].[CollectionEntryID] = [CollectionEntry].[CollectionEntryID]
		INNER JOIN [Collection] ON [CollectionEntry].[CollectionID] = [Collection].[CollectionID]
		WHERE [MarketEntry].[Active] = 1
		AND [Collection].[UserID] = @UserID
		AND [MarketEntry].[MarketEntryStatusID] IN ('Available', 'Sold')
	END
GO


print '' print '*** Creating sp_select_collection_entry_by_id'
GO
CREATE PROCEDURE [dbo].[sp_select_collection_entry_by_id]
	(
	@CollectionEntryID	[int]
	)
AS
	BEGIN
		SELECT [CollectionEntryID], [CollectionID], [CreatureID], [Name], [Level], 
				[Health], [Stamina], [Oxygen], [Food], [Weight], [BaseDamage], 
				[MovementSpeed], [Torpor], [Imprint], [Active]
		FROM [CollectionEntry]
		WHERE [CollectionEntryID] = @CollectionEntryID
	END
GO
		
		
print '' print '*** Creating sp_update_collection_entry_active'
GO
CREATE PROCEDURE [dbo].[sp_update_collection_entry_active]
	(
	@CollectionEntryID	[int],
	@Active			[bit]
	)
AS
	BEGIN
		UPDATE [CollectionEntry]
		SET [Active] = @Active
		WHERE [CollectionEntryID] = @CollectionEntryID
		RETURN @@ROWCOUNT
	END
GO


print '' print '*** Creating sp_select_user_by_market_entry_id'
GO
CREATE PROCEDURE [dbo].[sp_select_user_by_market_entry_id]
	(
	@MarketEntryID  [int]
	)
AS
	BEGIN
		SELECT [User].[UserID], [User].[Gamertag], [User].[FirstName], [User].[LastName], [User].[PhoneNumber], [User].[Email], [User].[Active]
		FROM [User]
		INNER JOIN [Collection] ON [User].[UserID] = [Collection].[UserID]
		INNER JOIN [CollectionEntry] ON [Collection].[CollectionID] = [CollectionEntry].[CollectionID]
		INNER JOIN [MarketEntry] ON [CollectionEntry].[CollectionEntryID] = [MarketEntry].[CollectionEntryID]
		WHERE [MarketEntry].[MarketEntryID] = @MarketEntryID
	END
GO


print '' print '*** Creating sp_update_market_entry_status'
GO
CREATE PROCEDURE [dbo].[sp_update_market_entry_status]
	(
	@MarketEntryID			[int],
	@NewMarketEntryStatusID	[nvarchar](30),
	@OldMarketEntryStatusID	[nvarchar](30)
	)
AS
	BEGIN
		UPDATE [MarketEntry]
		SET [MarketEntryStatusID] = @NewMarketEntryStatusID,
			[Active] = (SELECT [dbo].[fn_get_market_entry_active](@NewMarketEntryStatusID))
		WHERE [MarketEntryID] = @MarketEntryID
		AND [MarketEntryStatusID] = @OldMarketEntryStatusID
		RETURN @@ROWCOUNT
	END
GO


print '' print '*** Creating sp_create_market_entry_purchase'
GO
CREATE PROCEDURE [dbo].[sp_create_market_entry_purchase]
	(
	@UserID			[int],			
	@MarketEntryID	[int]
	)
AS
	BEGIN
		INSERT INTO [dbo].[MarketEntryPurchase]
			(
			[UserID], [MarketEntryID]
			)
		VALUES
			(@UserID, @MarketEntryID)
	END
GO
		
	
print '' print '*** Creating sp_perform_market_entry_purchase'
GO
CREATE PROCEDURE [dbo].[sp_perform_market_entry_purchase]
	(
	@UserID			[int],			
	@MarketEntryID	[int]
	)
AS
	BEGIN
		DECLARE @inserr int
		DECLARE @upderr int
		DECLARE @maxerr int
		DECLARE @err_message nvarchar(255)
		
		SET @maxerr = 0
	
		BEGIN TRANSACTION
		
		/*Create market entry purchase record*/
		INSERT INTO [dbo].[MarketEntryPurchase]
			(
			[UserID], [MarketEntryID]
			)
		VALUES
			(@UserID, @MarketEntryID)
	
		/*Check for error*/
		SET @inserr = @@error
		IF @inserr > @maxerr
			SET @maxerr = @inserr
		
		/*Update the market entry to sold*/
		UPDATE [MarketEntry]
		SET [MarketEntryStatusID] = 'Sold'
		WHERE [MarketEntryID] = @MarketEntryID
		--AND [MarketEntryStatusID] = 'Available'
		
		/*Check for errors updating*/
		SET @upderr = @@error
		IF @upderr > @maxerr
			SET @maxerr = @upderr
		IF @inserr <> 0
			BEGIN 
				ROLLBACK
				SET @err_message = 'Could not insert a new market entry purchase'
				RAISERROR (@err_message, 20, 1)
			END
		ELSE IF @upderr <> 0
			BEGIN
				ROLLBACK
				SET @err_message = 'Could not update market entry to Sold'
				RAISERROR (@err_message, 20, 1)
			END
		ELSE
			BEGIN
				COMMIT
				RETURN @@ROWCOUNT
			END
		
	END
GO


print '' print '*** Creating sp_select_market_entry_purchases_by_user'
GO
CREATE PROCEDURE [dbo].[sp_select_market_entry_purchases_by_user]
	(
	@UserID		[int]
	)
AS
	BEGIN
		SELECT [MarketEntryPurchase].[UserID], [MarketEntryPurchase].[MarketEntryID]
		FROM [MarketEntryPurchase]
		INNER JOIN [MarketEntry] ON [MarketEntryPurchase].[MarketEntryID] = [MarketEntry].[MarketEntryID]
		WHERE [MarketEntryPurchase].[UserID] = @UserID
		AND [MarketEntry].[MarketEntryStatusID] = 'Sold'
	END
GO
	
	
print '' print '*** Creating sp_select_market_entry_by_id'
GO
CREATE PROCEDURE [dbo].[sp_select_market_entry_by_id]
	(
	@MarketEntryID		[int]
	)
AS
	BEGIN
		SELECT [MarketEntryID], [CollectionEntryID], [MarketEntryStatusID], [ResourceID], [Units], [Active]
		FROM [MarketEntry]
		WHERE [MarketEntryID] = @MarketEntryID
	END
GO
		
		
print '' print '*** Creating sp_retrieve_user_by_id'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_user_by_id]
	(
	@UserID	[int]
	)
AS
	BEGIN
		SELECT [UserID], [Gamertag], [FirstName], [LastName], [PhoneNumber], [Email], [Active]
		FROM [User]
		WHERE [UserID] = @UserID
	END
GO
		
		
print '' print '*** Creating sp_retrieve_resources_by_active'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_resources_by_active]
	(
	@Active	[bit]
	)
AS
	BEGIN
		SELECT [ResourceID], [Active]
		FROM [Resource]
		WHERE [Active] = @Active
	END
GO


print '' print '*** Creating sp_retrieve_market_entry_count_by_collection_entry_id'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_market_entry_count_by_collection_entry_id]
	(
	@CollectionEntryID	[int]
	)
AS
	BEGIN
		SELECT COUNT([MarketEntryID])
		FROM [MarketEntry]
		WHERE [CollectionEntryID] = @CollectionEntryID
		AND [MarketEntryStatusID] IN ('Available', 'Sold')
		
	END
GO


print '' print '*** Creating sp_create_market_entry'
GO
CREATE PROCEDURE [dbo].[sp_create_market_entry]
	(
	@CollectionEntryID	[int],
	@ResourceID			[nvarchar](30),
	@Units				[int]
	)
AS
	BEGIN
		INSERT INTO [MarketEntry]
			([CollectionEntryID], [ResourceID], [Units], [MarketEntryStatusID])
		VALUES
			(@CollectionEntryID, @ResourceID, @Units, 'Available')
		
	END
GO
		
		
print '' print '*** Creating sp_update_market_entry'
GO
CREATE PROCEDURE [dbo].[sp_update_market_entry]
	(
	@MarketEntryID	[int],
	@OldResourceID	[nvarchar](30),
	@NewResourceID	[nvarchar](30),
	@OldUnits		[int],
	@NewUnits		[int]
	)
AS
	BEGIN
		UPDATE [MarketEntry]
			SET [ResourceID] = @NewResourceID,
				[Units] = @NewUnits
			WHERE [MarketEntryID] = @MarketEntryID
			AND [ResourceID] = @OldResourceID
			AND [Units] = @OldUnits
	END
GO
		

print '' print '*** Creating sp_retrieve_message_by_market_entry_id'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_message_by_market_entry_id]
	(
	@MarketEntryID	[int]
	)
AS
	BEGIN
		SELECT [MessageID], [UserID], [MarketEntryID], [Text], [SentTime]
		FROM [Message]
		WHERE [MarketEntryID] = @MarketEntryID
		ORDER BY [SentTime] ASC
	END
GO


print '' print '*** Creating sp_create_message'
GO
CREATE PROCEDURE [dbo].[sp_create_message]
	(
	@UserID			[int],
	@MarketEntryID	[int],
	@Text			[nvarchar](150),
	@SentTime		[datetime]
	)
AS
	BEGIN
		INSERT INTO [Message]
			([UserID], [MarketEntryID], [Text], [SentTime])
		VALUES
			(@UserID, @MarketEntryID, @Text, @SentTime)
	END
GO


print '' print '*** Creating retrieve_user_by_market_entry_purchase_market_entry_id'
GO
CREATE PROCEDURE [dbo].[retrieve_user_by_market_entry_purchase_market_entry_id]
	(
	@MarketEntryID [int]
	)
AS
	BEGIN
	
		
		SELECT [User].[UserID], [User].[Gamertag], [User].[FirstName], [User].[LastName], [User].[PhoneNumber], [User].[Email], [User].[Active]
		FROM [User]
		INNER JOIN [MarketEntryPurchase]
		ON [User].[UserID] = [MarketEntryPurchase].[UserID]
		INNER JOIN [MarketEntry]
		ON [MarketEntryPurchase].[MarketEntryID] = [MarketEntry].[MarketEntryID]
		WHERE [MarketEntryPurchase].[MarketEntryID] = @MarketEntryID
		AND [MarketEntry].[MarketEntryStatusID] = 'Sold' 
	END
GO


print '' print '*** Creating sp_perform_market_entry_complete'
GO
CREATE PROCEDURE [dbo].[sp_perform_market_entry_complete]
	(
	-- Creature data
	@CollectionID		[int],			
	@CreatureID		[nvarchar](50),	
	@Name			[nvarchar](50),
	@Level			[int],			
	@Health			[int],			
	@Stamina		[int],			
	@Oxygen			[int],			
	@Food			[int],			
	@Weight			[int],			
	@BaseDamage		[int],			
	@MovementSpeed	[int],			
	@Torpor			[int],			
	@Imprint		[int],			
	@Active			[bit],
	-- Market Entry data
	@MarketEntryID [int],
	@CollectionEntryID	[int]
	)
AS
	BEGIN
		DECLARE @CollectionDeactivateErr [int]
		DECLARE @CollectionEntryAddErr [int]
		DECLARE @MarketEntryUpdateErr [int]
		DECLARE @maxerr [int]
		DECLARE @err_message [nvarchar](255)
		
		SET @maxerr = 0
	
		BEGIN TRANSACTION
		
		/*Deactivate the collection*/
		UPDATE [CollectionEntry]
		SET [Active] = 0
		WHERE [CollectionEntryID] = @CollectionEntryID
	
		/*Check for error*/
		SET @CollectionDeactivateErr = @@error
		IF @CollectionDeactivateErr > @maxerr
			SET @maxerr = @CollectionDeactivateErr
		
		/*Add the collection to the new user's collection*/
		INSERT INTO [dbo].[CollectionEntry]
				(
				[CollectionID],
				[CreatureID],
				[Name],
				[Level],
				[Health],
				[Stamina],
				[Oxygen],
				[Food],
				[Weight],
				[BaseDamage],
				[MovementSpeed],
				[Torpor],
				[Imprint],
				[Active]
				)
		VALUES(
				@CollectionID,			
				@CreatureID,	
				@Name,
				@Level,			
				@Health,			
				@Stamina,			
				@Oxygen,			
				@Food,			
				@Weight,			
				@BaseDamage,			
				@MovementSpeed,			
				@Torpor,			
				@Imprint,			
				@Active
			)
		
		/*Check for errors inserting*/
		SET @CollectionEntryAddErr = @@error
		IF @CollectionEntryAddErr > @maxerr
			SET @maxerr = @CollectionEntryAddErr
			
		/*Update the market entry to reflect the proper status*/
		UPDATE [MarketEntry]
		SET [Active] = 0, [MarketEntryStatusID] = 'Complete'
		WHERE [MarketEntryID] = @MarketEntryID
	
		/*Check for error*/
		SET @MarketEntryUpdateErr = @@error
		IF @MarketEntryUpdateErr > @maxerr
			SET @maxerr = @MarketEntryUpdateErr	
			
		IF @CollectionDeactivateErr <> 0
			BEGIN 
				ROLLBACK
				SET @err_message = 'Could not deactivate the collection'
				RAISERROR (@err_message, 20, 1)
			END
		ELSE IF @CollectionEntryAddErr <> 0
			BEGIN
				ROLLBACK
				SET @err_message = 'Could not add the collection to the new user'
				RAISERROR (@err_message, 20, 1)
			END
		ELSE IF @MarketEntryUpdateErr <> 0
			BEGIN
				ROLLBACK
				SET @err_message = 'Could not update the market entry to the correct status'
				RAISERROR (@err_message, 20, 1)
			END
		ELSE
			BEGIN
				COMMIT
				RETURN @@ROWCOUNT
			END
		
	END
GO


print '' print '*** Creating sp_retrieve_creatures'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_creatures]
AS
	BEGIN
	
		SELECT [CreatureID], [CreatureTypeID], [CreatureDietID], [Active]
		FROM [Creature]
		
	END
GO


print '' print '*** Creating sp_add_creature'
GO
CREATE PROCEDURE [dbo].[sp_add_creature]
	(
	@CreatureID	[nvarchar](50),
	@CreatureTypeID	[nvarchar](30),
	@CreatureDietID	[nvarchar](30)
	)
AS
	BEGIN
		INSERT INTO [Creature]
			([CreatureID], [CreatureTypeID], [CreatureDietID])
		VALUES
			(@CreatureID, @CreatureTypeID, @CreatureDietID)
		
	END
GO


print '' print '*** Creating sp_update_creature_active'
GO
CREATE PROCEDURE [dbo].[sp_update_creature_active]
	(
	@CreatureID	[nvarchar](50),
	@Active		[bit]
	)
AS
	BEGIN
		UPDATE [Creature]
			SET [Active] = @Active
		WHERE [CreatureID] = @CreatureID
		
	END
GO


print '' print '*** Creating sp_update_creature'
GO
CREATE PROCEDURE [dbo].[sp_update_creature]
	(
	@OldCreatureID	[nvarchar](50),
	@NewCreatureID	[nvarchar](50),
	@OldCreatureTypeID	[nvarchar](30),
	@OldCreatureDietID	[nvarchar](30),
	@NewCreatureTypeID	[nvarchar](30),
	@NewCreatureDietID	[nvarchar](30)
	)
AS
	BEGIN
		UPDATE [Creature]
			SET [CreatureDietID] = @NewCreatureDietID, [CreatureTypeID] = @NewCreatureTypeID, [CreatureID] = @NewCreatureID
		WHERE [CreatureDietID] = @OldCreatureDietID
		AND [CreatureTypeID] = @OldCreatureTypeID
		AND [CreatureID] = @OldCreatureID
		
	END
GO


print '' print '*** Creating sp_retrieve_creatures_types'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_creatures_types]
AS
	BEGIN
	
		SELECT [CreatureTypeID], [Active]
		FROM [CreatureType]
		
	END
GO


print '' print '*** Creating sp_retrieve_creatures_types_active'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_creatures_types_active]
AS
	BEGIN
	
		SELECT [CreatureTypeID], [Active]
		FROM [CreatureType]
		WHERE [Active] = 1
		
	END
GO


print '' print '*** Creating sp_retrieve_creatures_type_by_id'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_creatures_type_by_id]
	(
		@CreatureTypeID	[nvarchar](30)
	)
AS
	BEGIN
	
		SELECT [CreatureTypeID], [Active]
		FROM [CreatureType]
		WHERE [CreatureTypeID] = @CreatureTypeID
		
	END
GO


print '' print '*** Creating sp_update_creature_type'
GO
CREATE PROCEDURE [dbo].[sp_update_creature_type]
	(
	@OldCreatureTypeID	[nvarchar](30),
	@NewCreatureTypeID	[nvarchar](30)
	)
AS
	BEGIN
		UPDATE [CreatureType]
			SET [CreatureTypeID] = @NewCreatureTypeID
		WHERE [CreatureTypeID] = @OldCreatureTypeID
		
	END
GO


print '' print '*** Creating sp_add_creature_type'
GO
CREATE PROCEDURE [dbo].[sp_add_creature_type]
	(
	@CreatureTypeID	[nvarchar](30)
	)
AS
	BEGIN
		INSERT INTO [CreatureType]
			([CreatureTypeID])
		VALUES
			(@CreatureTypeID)
		
	END
GO


print '' print '*** Creating sp_update_creature_type_active'
GO
CREATE PROCEDURE [dbo].[sp_update_creature_type_active]
	(
	@CreatureTypeID	[nvarchar](30),
	@Active		[bit]
	)
AS
	BEGIN
		UPDATE [CreatureType]
			SET [Active] = @Active
		WHERE [CreatureTypeID] = @CreatureTypeID
		
	END
GO


print '' print '*** Creating sp_retrieve_creatures_diet'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_creatures_diet]
AS
	BEGIN
	
		SELECT [CreatureDietID], [Active]
		FROM [CreatureDiet]
		
	END
GO

print '' print '*** Creating sp_retrieve_creatures_diet_by_id'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_creatures_diet_by_id]
(
	@CreatureDietID [nvarchar](30)
)
AS
	BEGIN
	
		SELECT [CreatureDietID], [Active]
		FROM [CreatureDiet]
		WHERE [CreatureDietID] = @CreatureDietID
		
	END
GO

print '' print '*** Creating sp_retrieve_creatures_diet_active'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_creatures_diet_active]
AS
	BEGIN
	
		SELECT [CreatureDietID], [Active]
		FROM [CreatureDiet]
		WHERE [Active] = 1
		
	END
GO


print '' print '*** Creating sp_update_creature_diet'
GO
CREATE PROCEDURE [dbo].[sp_update_creature_diet]
	(
	@OldCreatureDietID	[nvarchar](30),
	@NewCreatureDietID	[nvarchar](30)
	)
AS
	BEGIN
		UPDATE [CreatureDiet]
			SET [CreatureDietID] = @NewCreatureDietID
		WHERE [CreatureDietID] = @OldCreatureDietID
		
	END
GO


print '' print '*** Creating sp_add_creature_diet'
GO
CREATE PROCEDURE [dbo].[sp_add_creature_diet]
	(
	@CreatureDietID	[nvarchar](30)
	)
AS
	BEGIN
		INSERT INTO [CreatureDiet]
			([CreatureDietID])
		VALUES
			(@CreatureDietID)
		
	END
GO


print '' print '*** Creating sp_update_creature_diet_active'
GO
CREATE PROCEDURE [dbo].[sp_update_creature_diet_active]
	(
	@CreatureDietID	[nvarchar](30),
	@Active		[bit]
	)
AS
	BEGIN
		UPDATE [CreatureDiet]
			SET [Active] = @Active
		WHERE [CreatureDietID] = @CreatureDietID
		
	END
GO

	
print '' print '*** Creating sp_retrieve_resources'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_resources]
AS
	BEGIN
	
		SELECT [ResourceID], [Active]
		FROM [Resource]
		
	END
GO


print '' print '*** Creating sp_update_resource'
GO
CREATE PROCEDURE [dbo].[sp_update_resource]
	(
	@OldResourceID	[nvarchar](30),
	@NewResourceID	[nvarchar](30)
	)
AS
	BEGIN
		UPDATE [Resource]
			SET [ResourceID] = @NewResourceID
		WHERE [ResourceID] = @OldResourceID
		
	END
GO


print '' print '*** Creating sp_add_resource'
GO
CREATE PROCEDURE [dbo].[sp_add_resource]
	(
	@ResourceID	[nvarchar](30)
	)
AS
	BEGIN
		INSERT INTO [Resource]
			([ResourceID])
		VALUES
			(@ResourceID)
		
	END
GO


print '' print '*** Creating sp_update_resource_active'
GO
CREATE PROCEDURE [dbo].[sp_update_resource_active]
	(
	@ResourceID	[nvarchar](30),
	@Active		[bit]
	)
AS
	BEGIN
		UPDATE [Resource]
			SET [Active] = @Active
		WHERE [ResourceID] = @ResourceID
		
	END
GO

print '' print '*** Creating sp_create_user'
GO
CREATE PROCEDURE [dbo].[sp_create_user]
	(
		@Gamertag		[nvarchar](30),
		@FirstName		[nvarchar](50),
		@LastName		[nvarchar](100),
		@PhoneNumber	[nvarchar](15) = null,
		@Email			[nvarchar](100),
		@PasswordHash	[nvarchar](100)
	)
AS
	BEGIN
		INSERT INTO [User]
			([Gamertag], [FirstName], [LastName], [PhoneNumber], [Email], [PasswordHash])
		VALUES
			(@Gamertag, @FirstName, @LastName, @PhoneNumber, @Email, @PasswordHash)
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** Creating sp_select_collection_by_id'
GO
CREATE PROCEDURE [dbo].[sp_select_collection_by_id]
	(
	@CollectionID	[int]
	)
AS
	BEGIN
		SELECT [CollectionID], [UserID], [Name], [Active]
		FROM [Collection]
		WHERE [CollectionID] = @CollectionID
	END
GO


print '' print '*** Creating sp_retrieve_resource_by_id'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_resource_by_id]
	(
	@ResourceID	[nvarchar](30)
	)
AS
	BEGIN
		SELECT [ResourceID], [Active]
		FROM [Resource]
		WHERE [ResourceID] = @ResourceID
	END
GO


print '' print '*** Creating sp_select_market_entry_purchases_by_user'
GO
CREATE PROCEDURE [dbo].[sp_select_market_entry_purchases_by_user_2]
	(
	@UserID		[int]
	)
AS
	BEGIN
		SELECT [CollectionEntry].[CreatureID], [CollectionEntry].[Level], [CollectionEntry].[Name], [User].[Gamertag], [MarketEntry].[MarketEntryID], [MarketEntry].[ResourceID], [MarketEntry].[Units]
		FROM [MarketEntryPurchase]
		INNER JOIN [MarketEntry] ON [MarketEntryPurchase].[MarketEntryID] = [MarketEntry].[MarketEntryID]
		INNER JOIN [CollectionEntry] ON [MarketEntry].[CollectionEntryID] = [CollectionEntry].[CollectionEntryID]
		INNER JOIN [Collection] ON [CollectionEntry].[CollectionID] = [Collection].[CollectionID]
		INNER JOIN [User] ON [Collection].[UserID] = [User].[UserID]
		WHERE [MarketEntryPurchase].[UserID] = @UserID
		AND [MarketEntry].[MarketEntryStatusID] = 'Sold'
	END
GO


print '' print '*** Creating sp_select_market_entry_details_by_id'
GO
CREATE PROCEDURE [dbo].[sp_select_market_entry_details_by_id]
	(
		@MarketEntryID		[int]
	)
AS
	BEGIN
		SELECT [MarketEntry].[MarketEntryID], [MarketEntry].[CollectionEntryID], [CollectionEntry].[CreatureID], [CollectionEntry].[Name], [CollectionEntry].[Level], [CollectionEntry].[Health], [CollectionEntry].[Stamina], [CollectionEntry].[Oxygen], [CollectionEntry].[Food], [CollectionEntry].[Weight], [CollectionEntry].[BaseDamage], [CollectionEntry].[MovementSpeed], [CollectionEntry].[Torpor], [CollectionEntry].[Imprint], [CollectionEntry].[Active], [MarketEntry].[ResourceID], [MarketEntry].[Units], [MarketEntry].[MarketEntryStatusID]
		FROM [MarketEntry]
		INNER JOIN [CollectionEntry] ON [MarketEntry].[CollectionEntryID] = [CollectionEntry].[CollectionEntryID]
		WHERE [MarketEntry].[MarketEntryID] = @MarketEntryID
	END
GO


print '' print '*** Creating sp_select_market_entries_by_user_2'
GO
CREATE PROCEDURE [dbo].[sp_select_market_entries_by_user_2]
	(
		@UserID		[int]
	)
AS
	BEGIN
		SELECT [MarketEntry].[MarketEntryID], [MarketEntry].[CollectionEntryID], [CollectionEntry].[CreatureID], [CollectionEntry].[Name], [CollectionEntry].[Level], [CollectionEntry].[Health], [CollectionEntry].[Stamina], [CollectionEntry].[Oxygen], [CollectionEntry].[Food], [CollectionEntry].[Weight], [CollectionEntry].[BaseDamage], [CollectionEntry].[MovementSpeed], [CollectionEntry].[Torpor], [CollectionEntry].[Imprint], [CollectionEntry].[Active], [MarketEntry].[ResourceID], [MarketEntry].[Units], [MarketEntry].[MarketEntryStatusID]
		FROM [MarketEntry]
		INNER JOIN [CollectionEntry] ON [MarketEntry].[CollectionEntryID] = [CollectionEntry].[CollectionEntryID]
		INNER JOIN [Collection] ON [CollectionEntry].[CollectionID] = [Collection].[CollectionID]
		WHERE [MarketEntry].[Active] = 1
		AND [Collection].[UserID] = @UserID
		AND [MarketEntry].[MarketEntryStatusID] IN ('Available', 'Sold')
	END
GO


print '' print '*** Creating sp_select_collection_entries_by_user_id'
GO
CREATE PROCEDURE [dbo].[sp_select_collection_entries_by_user_id]
	(
	@UserID		[int]
	)
AS
	BEGIN
		
		SELECT [CollectionEntry].[CollectionEntryID]
		INTO #AlreadyOnMarket
		FROM [CollectionEntry], [MarketEntry]
		WHERE [CollectionEntry].[CollectionEntryID] = [MarketEntry].[CollectionEntryID]
		AND [MarketEntry].[MarketEntryStatusID] IN ('Available', 'Sold', 'Complete')
		
			
	
		SELECT [CollectionEntry].[CollectionEntryID], [CollectionEntry].[CollectionID], [CollectionEntry].[CreatureID], [CollectionEntry].[Name], [Level], 
				[CollectionEntry].[Health], [CollectionEntry].[Stamina], [CollectionEntry].[Oxygen], [CollectionEntry].[Food], [CollectionEntry].[Weight], [CollectionEntry].[BaseDamage], 
				[CollectionEntry].[MovementSpeed], [CollectionEntry].[Torpor], [CollectionEntry].[Imprint], [CollectionEntry].[Active]
		FROM [CollectionEntry]
		INNER JOIN [Collection] ON [CollectionEntry].[CollectionID] = [Collection].[CollectionID]
		WHERE [Collection].[UserID] = @UserID
		AND [CollectionEntry].[Active] = 1
		AND [CollectionEntry].[CollectionEntryID] NOT IN 
			(
				SELECT [CollectionEntryID] FROM #AlreadyOnMarket
			)
		
	END
GO




print '' print '*** Creating sp_select_market_entry_details_by_available'
GO
CREATE PROCEDURE [dbo].[sp_select_market_entry_details_by_available]
AS
	BEGIN
		SELECT [MarketEntry].[MarketEntryID], [MarketEntry].[CollectionEntryID], [CollectionEntry].[CreatureID], [CollectionEntry].[Name], [CollectionEntry].[Level], [CollectionEntry].[Health], [CollectionEntry].[Stamina], [CollectionEntry].[Oxygen], [CollectionEntry].[Food], [CollectionEntry].[Weight], [CollectionEntry].[BaseDamage], [CollectionEntry].[MovementSpeed], [CollectionEntry].[Torpor], [CollectionEntry].[Imprint], [CollectionEntry].[Active], [MarketEntry].[ResourceID], [MarketEntry].[Units], [MarketEntry].[MarketEntryStatusID], [User].[Gamertag], [User].[UserID]
		FROM [MarketEntry]
		INNER JOIN [CollectionEntry] ON [MarketEntry].[CollectionEntryID] = [CollectionEntry].[CollectionEntryID]
		INNER JOIN [Collection] ON [CollectionEntry].[CollectionID] = [Collection].[CollectionID]
		INNER JOIN [User] ON [Collection].[UserID] = [User].[UserID]
		WHERE [MarketEntry].[MarketEntryStatusID] = 'Available'
	END
GO
