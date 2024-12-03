-- Insert dummy likes 
INSERT INTO Likes (LikerID, LikedID) 
VALUES (1, 2), (3, 4), (2, 3), (4, 1), (6, 5), (7, 8); 
-- Insert dummy dislikes 
INSERT INTO Dislikes (DislikerID, DislikedID) 
VALUES (1, 3), (2, 4), (3, 5), (4, 6), (5, 1), (6, 2), (7, 9), (8, 10);

INSERT INTO Matches (Visitor_VisitorID1, Visitor_VisitorID2) 
SELECT v1.VisitorID, v2.VisitorID FROM Visitor v1 JOIN Visitor v2 ON v1.VisitorID < v2.VisitorID 
LEFT JOIN Likes l1 ON v1.VisitorID = l1.LikerID AND v2.VisitorID = l1.LikedID 
LEFT JOIN Likes l2 ON v2.VisitorID = l2.LikerID AND v1.VisitorID = l2.LikedID 
LEFT JOIN Dislikes d1 ON v1.VisitorID = d1.DisLikerID AND v2.VisitorID = d1.DislikedID 
LEFT JOIN Dislikes d2 ON v2.VisitorID = d2.DisLikerID AND v1.VisitorID = d2.DislikedID 
WHERE (l1.LikerID IS NULL AND l2.LikerID IS NULL) AND (d1.DisLikerID IS NULL AND d2.DisLikerID IS NULL) ORDER BY RAND() 
LIMIT 50;