CREATE TABLE Appointments (
    AppointmentID BIGINT(128) NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Visitor_VisitorID1 BIGINT(128) NOT NULL,
    Visitor_VisitorID2 BIGINT(128) NOT NULL,
    Start DATETIME NOT NULL,
    End DATETIME NOT NULL,
    Accepted BOOL DEFAULT false,
    CONSTRAINT FK_VisitorID1 FOREIGN KEY (Visitor_VisitorID1) REFERENCES Visitor(VisitorID),
    CONSTRAINT FK_VisitorID2 FOREIGN KEY (Visitor_VisitorID2) REFERENCES Visitor(VisitorID)
);