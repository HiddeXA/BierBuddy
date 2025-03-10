﻿using NUnit.Framework;
using Moq;
using System;
using BierBuddy.Core;

[TestFixture]
public class SwitchClientProfileTests
{
    private Mock<IDataAccess> _dataAccessMock;
    private Main _main;
    private SwitchAccount _switchAccount;

    [SetUp]
    public void SetUp()
    {
        Visitor Dummy = new Visitor(1, "BierBuddyBram", "De trotse mascotte van BierBuddy. Hij is zo cool, hij kan zelfs 500 bier opdrinken in een uur. Niemand is beter dan deze makker.", 21);
        Dummy.AddToDrinkPreference("Jägermeister");
        Dummy.AddToDrinkPreference("Hertog Jan");
        Dummy.AddToDrinkPreference("Speciaal bier");
        Dummy.AddToDrinkPreference("Dikke Vette Drank");
        Dummy.AddToInterests("Bierbrouwen");
        Dummy.AddToInterests("Jagen");
        Dummy.AddToInterests("Skiën");
        Dummy.AddToActivityPreference("Gezellig kletsen");
        Dummy.AddToActivityPreference("Darten");

        _dataAccessMock = new Mock<IDataAccess>();
        _main = new Main(_dataAccessMock.Object, Dummy);
        _switchAccount = new SwitchAccount(_dataAccessMock.Object, _main);
    }

    [Test]
    public void SwitchClientProfile_ValidAccount_UpdatesClientVisitorAndFiresEvent()
    {
        // Arrange
        long validAccountID = 2;
        Visitor expectedVisitor = new Visitor(validAccountID, "Jan", "Ik ben Jan", 30);

        _dataAccessMock.Setup(x => x.GetAccount(validAccountID)).Returns(expectedVisitor);

        bool eventFired = false;
        long eventAccountID = 0;
        _switchAccount.OnClientProfileChanged += (sender, ClientProfileChangedEventArgs) =>
        {
            eventFired = true;
            eventAccountID = ClientProfileChangedEventArgs.NewClientProfileID;
        };

        // Act
        _switchAccount.SwitchClientProfile(validAccountID.ToString());

        // Assert
        Assert.That(_main.ClientVisitor, Is.EqualTo(expectedVisitor));
        Assert.IsTrue(eventFired);
        Assert.That(eventAccountID, Is.EqualTo(validAccountID));
    }

    [Test]
    public void SwitchClientProfile_InvalidAccount_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        long invalidAccountID = 99;

        _dataAccessMock.Setup(x => x.GetAccount(invalidAccountID)).Returns((Visitor?)null);

        // Act & Assert
        Exception ex = Assert.Throws<ArgumentOutOfRangeException>(() => _switchAccount.SwitchClientProfile(invalidAccountID.ToString()));
        Assert.That(ex.GetType(), Is.EqualTo(typeof(ArgumentOutOfRangeException)));
    }

    [Test]
    public void SwitchClientProfile_NonNumericInput_ThrowsFormatException()
    {
        // Arrange
        string invalidInput = "invalid_id";

        // Act & Assert
        Exception ex = Assert.Throws<FormatException>(() => _switchAccount.SwitchClientProfile(invalidInput));
        Assert.That(ex.GetType(), Is.EqualTo(typeof(FormatException)));
    }
}

