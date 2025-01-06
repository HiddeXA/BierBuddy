ALTER TABLE visitor ADD Email varchar(256) NOT NULL;
UPDATE visitor SET Email = 'generic@mail.test';
ALTER TABLE visitor ADD Passkey varchar(128) NOT NULL;
UPDATE visitor SET Passkey = '6173b17ce1941d01438af6a5c12d301fcbcdbf36ba87fcc0627b7461d4e1ffc9b21e0b1495eec507d27c7f71043eac286576e0373702816a2d37b15b9bd94c2f';
