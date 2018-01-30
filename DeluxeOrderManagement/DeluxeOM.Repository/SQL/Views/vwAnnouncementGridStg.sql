CREATE VIEW [dbo].[vwAnnouncementGridStg]
AS

SELECT  
		ROW_NUMBER() OVER (ORDER BY Title) AS RowNum,
        Title, 
		MPM,
		[Video Version], 
		(CASE WHEN ([Local Edit] IS NULL OR [Local Edit] = '') THEN 'No' ELSE [Local Edit] END) AS [Local Edit], 
		[Version EIDR], 
		Category, 
		[Primary VID], 
		[Vendor ID], 
		Region, 
		Territory, 
		Language, 
		[Language Type], 
		[Pre-Order SD Title Status], 
		[Pre-Order HD Title Status], 
        [EST SD Title Status], 
		[EST HD Title Status], 
		[VOD SD Title Status], 
		[VOD HD Title Status], 
		[Announcement Pre Order SD Date], 
		[Announcement Pre Order HD Date], 
		[Announcement EST SD Start], 
        [Announcement EST HD Start], 
		[Announcement VOD SD Start], 
		[Announcement VOD HD Start], 
		[First Announced Date], 
		[Last Announcement Date],
		[Pre-Order 4k Title Status],
		[EST 4k Title Status],
		[VOD 4k Title Status],
		[Announcement Pre Order 4k Date],
		[Announcement EST 4k Start],
		[Announcement VOD 4K Start]
FROM
(SELECT DISTINCT 
        Title, 
		MPM,
		[Video Version], 
		[Local Edit], 
		[Version EIDR], 
		Category, 
		[Primary VID], 
		[Vendor ID], 
		Region, 
		Territory, 
		[Language], 
		[Language Type], 
		[Pre-Order SD Title Status], 
		[Pre-Order HD Title Status], 
        [EST SD Title Status], 
		[EST HD Title Status], 
		[VOD SD Title Status], 
		[VOD HD Title Status], 
		[Announcement Pre Order SD Date], 
		[Announcement Pre Order HD Date], 
		[Announcement EST SD Start], 
        [Announcement EST HD Start], 
		[Announcement VOD SD Start], 
		[Announcement VOD HD Start], 
		[First Announced Date], 
		[Last Announcement Date],
		[Pre-Order 4k Title Status],
		[EST 4k Title Status],
		[VOD 4k Title Status],
		[Announcement Pre Order 4k Date],
		[Announcement EST 4k Start],
		[Announcement VOD 4K Start]
FROM            dbo.AppleGrid_STAGING) STG



--OrderGrid
