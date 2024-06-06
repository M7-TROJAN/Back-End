-- Creating or altering a view to display questions along with user details
CREATE OR ALTER VIEW dbo.QuestionsWithUsers
AS 
    SELECT
        P.Id AS PostID,          -- Assigning Post's ID to PostID
        P.OwnerUserId AS UserId, -- Assigning Owner's ID to UserId
        U.DisplayName AS UserName, -- Assigning User's Display Name to UserName
        P.CreationDate,         -- Date of Post Creation
        P.Body                  -- Post Content
    FROM
        dbo.Posts P
    INNER JOIN
        dbo.Users U
    ON
        P.OwnerUserId = U.Id    -- Joining Posts with Users based on the Owner's ID
    WHERE
        P.PostTypeId = 1        -- Filtering only Questions (Post Type ID = 1)
GO

-- Creating a unique clustered index for optimization
CREATE UNIQUE CLUSTERED INDEX Post_IX ON dbo.QuestionsWithUsers(PostId)

-- Retrieving data from the created view
SELECT * FROM dbo.QuestionsWithUsers;
