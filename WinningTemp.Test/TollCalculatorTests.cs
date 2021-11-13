using System;
using System.Collections.Generic;
using Xunit;

namespace WinningTemp.Test
{
    public class TollCalculatorTests
    {
        [Fact]
        public void GetTollFee_Should_NotThrowExceptions_When_DatesIsNull()
        {
            //Arrange
            var tollCalculator = new TollCalculator();
            
            //Act
            var amount = tollCalculator.GetTotalTollFee(new CarVehicle(), null);
        }
        
        [Fact]
        public void GetTollFee_Should_Charge_0_When_VehicleIsTollFree()
        {
            //Arrange
            var tollCalculator = new TollCalculator();
            var dates = new List<DateTime>()
            {
                new(2021,11,11)
            };
            
            //Act
            var amount = tollCalculator.GetTotalTollFee(new MotorbikeVehicle(), dates.ToArray());
            
            //Assert
            Assert.Equal(0,amount);
        }
        
        [Theory]
        [InlineData(1,1)]
        [InlineData(12,31)]
        public void GetTollFee_Should_Charge_0_When_Holiday(int month, int day)
        {
            //Arrange
            var tollCalculator = new TollCalculator();
            var dates = new List<DateTime>()
            {
                new(2021,month,day)
            };
            
            //Act
            var amount = tollCalculator.GetTotalTollFee(new CarVehicle(), dates.ToArray());
            
            //Assert
            Assert.Equal(0,amount);
        }
        
        [Fact]
        public void GetTollFee_Should_Charge_0_When_Weekend()
        {
            //Arrange
            var tollCalculator = new TollCalculator();
            var dates = new List<DateTime>()
            {
                new(2021,11,13)
            };
            
            //Act
            var amount = tollCalculator.GetTotalTollFee(new CarVehicle(), dates.ToArray());
            
            //Assert
            Assert.Equal(0,amount);
        }
        
        [Theory]
        [InlineData(6,0,9)]
        [InlineData(6,29,9)] 
        [InlineData(15,30,22)] 
        public void GetTollFee_Should_Charge_CorrectAmount_When_OneDateIsPassed(int hour, int minute, int expectedAmount)
        {
            //Arrange
            var tollCalculator = new TollCalculator();
            var dates = new List<DateTime>()
            {
                new(2021,11,11, hour,minute,0)
            };
            
            //Act
            var amount = tollCalculator.GetTotalTollFee(new CarVehicle(), dates.ToArray());
            
            //Assert
            Assert.Equal(expectedAmount,amount);
        }
        
        [Fact] 
        public void GetTollFee_Should_Charge_60_When_TotalChargeIsMoreThan60()
        {
            //Arrange
            var tollCalculator = new TollCalculator();
            var dates = new List<DateTime>() //Total 69
            {
                new(2021,11,11, 6,0,0), //charge 9
                new(2021,11,11, 7,30,0), //charge 22
                new(2021,11,11, 10,0,0), //charge 16
                new(2021,11,11, 15,30,0) //charge 16
            };
            
            //Act
            var amount = tollCalculator.GetTotalTollFee(new CarVehicle(), dates.ToArray());
            
            //Assert
            Assert.Equal(60,amount);
        } 
        
        [Fact] 
        public void GetTollFee_Should_Charge_CorrectAmount_When_MultipleDatesArePassed()
        {
            //Arrange
            var tollCalculator = new TollCalculator();
            var dates = new List<DateTime>()
            {
                new(2021,11,11, 6,0,0), //charge 9
                new(2021,11,11, 15,30,0) //charge 22
            };
            
            //Act
            var amount = tollCalculator.GetTotalTollFee(new CarVehicle(), dates.ToArray());
            
            //Assert
            Assert.Equal(31,amount);
        }
        
        [Fact] 
        public void GetTollFee_Should_Charge_Once_When_MultipleDatesForOneHourInOneSlot()
        {
            //Arrange
            var tollCalculator = new TollCalculator();
            var dates = new List<DateTime>() //Total 32
            {
                new(2021,11,11, 15,0,0), //charge 16
                new(2021,11,11, 15,15,0), //charge 16
                new(2021,11,11, 15,25,0), //charge 16
            };
            
            //Act
            var amount = tollCalculator.GetTotalTollFee(new CarVehicle(), dates.ToArray());
            
            //Assert
            Assert.Equal(16,amount);
        }
        
        [Fact] 
        public void GetTollFee_Should_Charge_Once_When_MultipleDatesForOneHourInSiblingSlots()
        {
            //Arrange
            var tollCalculator = new TollCalculator();
            var dates = new List<DateTime>()
            {
                new(2021,11,11, 14,50,0), //charge 9
                new(2021,11,11, 15,15,0), //charge 16
                new(2021,11,11, 15,25,0), //charge 16
                new(2021,11,11, 16,00,0), //charge 22
            };
            
            //Act
            var amount = tollCalculator.GetTotalTollFee(new CarVehicle(), dates.ToArray());
            
            //Assert
            Assert.Equal(9 + 22,amount);
        }
    }
}