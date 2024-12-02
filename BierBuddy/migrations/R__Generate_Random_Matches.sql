INSERT INTO Matches (Visitor_VisitorID1 , Visitor_VisitorID2) 
SELECT v1.VisitorID, v2.VisitorID 
FROM Visitor v1 
JOIN Visitor v2 ON v1.VisitorID < v2.VisitorID 
ORDER BY RAND() 
LIMIT 50;