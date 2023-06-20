using NUnit.Framework;

namespace LinearExpansivityTests.Services
{
    [TestFixture]
    public class LinearExpansivityClassTests
    {
       // private ILinearExpansivity _ilinearExamsivity { get; set; }

        [SetUp]
        public void SetUp()
        {
          // _ilinearExamsivity = new LinearExpansivity();
        }
       

        [Test]
        [TestCase(1, 2, 1)]
        [TestCase(2, 1, 0)]
        [TestCase(0, 1, 0)]
        [TestCase(1, 0, 0)]
        [TestCase(-1, 1, 0)]
        [TestCase(1,-1, 0)]

        public void Change_WhenCalled_ReturnTheDifferenceBetweenTheArgs(double initial,double final, decimal expectedResult)
        {
            //Arrange
            var linearExpansivityClass = new LinearExpansivityClass();

            //Act
            var result = linearExpansivityClass.Change(initial, final);

            //Assert
            Assert.That(result, Is.EqualTo(expectedResult));

        }
        [Test]
        [TestCase(0, 0.00000008, 10, 200, 0.0000016)] //When the linear expansivity of the first material is unknown
        [TestCase(0.0000016, 0, 10, 200, 0.00000008)] //When the linear expansivity of the second material is unknown
        [TestCase(0.0000016, 0.00000008, 0, 200, 10)] //When the material-length  of first material is unknow
        [TestCase(0.0000016, 0.00000008, 10, 0, 200)] //When the material-length  of second material is unknow
        [TestCase(0, 0, 0, 0, 0)]                     //When all is unknown
        [TestCase(0, 1, 0, 1, 0)]                     //When having two or more unknown
        [TestCase(1, 0, 1, 0, 0)]
        [TestCase(0, 0, 0, 1, 0)]
        [TestCase(1, 0, 0, 0, 0)]
        public void LinearExpansivityOfTwoDifferentMaterialsAtSameTemprature_WhenCalled_ReturnTheResultOfTheUnknownArgThatisZero(double linearExpansivityOfMaterial_A, double linearExpansivityOfMaterial_B, double lengthOfMaterial_A_In_Meter, double lengthOfMaterial_B_In_Meter, decimal expectedResult)
        {
            //Arrange
            var linearExpansivityClass = new LinearExpansivityClass();

            //Act
            var result = linearExpansivityClass.LinearExpansivityOfTwoDifferentMaterialsAtSameTemprature(linearExpansivityOfMaterial_A, linearExpansivityOfMaterial_B, lengthOfMaterial_A_In_Meter, lengthOfMaterial_B_In_Meter);

            //Assert
            Assert.That(result, Is.EqualTo(expectedResult));

        }


    }

    public class LinearExpansivityClass
    {

        public decimal Change(double initial, double final) 
        {
            if (initial <= 0 || final <= 0 || initial > final) return 0;

            var result = final - initial;
            return (decimal)result;
        }

        public decimal SolvingForLinearExpansivity(double initialLengthInMeter = 0, double finalLengthInMeter = 0, double initialTempInKelvin = 0, double finalTempInKelvin = 0, double alpha = 0, string resultType = null)
        {
            //Checking if the user need the answer in per Celcius,else it will solve the answer in kelvin
            if (resultType == "Celcius")
            {
                initialTempInKelvin -= 374;
                finalTempInKelvin -= 374;
            }
            var changeInLength = Change(initialLengthInMeter, finalLengthInMeter);  //BitaLength
            var changeInTemperature = Change(initialTempInKelvin, finalTempInKelvin); //BitaTita
            //Checking if the parameters are in good condition
            if (initialLengthInMeter.Equals(0d) && finalLengthInMeter.Equals(0d) && initialTempInKelvin.Equals(0d) && finalTempInKelvin.Equals(0d)) return 0m;
            if (initialLengthInMeter < 0 ||
                finalLengthInMeter < 0 ) return 0m;
            if(initialLengthInMeter > finalLengthInMeter && !finalLengthInMeter.Equals(0))
            {
                return 0m;
            }
            if (initialTempInKelvin > finalTempInKelvin && !finalTempInKelvin.Equals(0))
            {
                return 0m;
            }
            if(finalLengthInMeter.Equals(0d))  //Final length is unknown
            {
                if (initialLengthInMeter.Equals(0d) && alpha.Equals(0d) && changeInTemperature.Equals(0m))
                {
                    return 0m;
                }
                decimal l2 = (decimal)initialLengthInMeter * (((decimal)alpha * changeInTemperature) + 1);  
                return l2; //Returning final length,when final length is unknown
            }
            if (initialLengthInMeter.Equals(0d))  //initial length is unknown
            {
                if (finalLengthInMeter.Equals(0d) && alpha.Equals(0d) && changeInTemperature.Equals(0m))
                {
                    return 0m;
                }
                var l1 = (decimal)finalLengthInMeter / (((decimal)alpha *  changeInTemperature) + 1);  
                return l1;  //Returning inital length,when initial length is unknown
            }
            if (finalTempInKelvin.Equals(0d))  //final Temperature is unknown
            {
                if (initialTempInKelvin.Equals(0d) && alpha.Equals(0d) && changeInLength.Equals(0m))
                {
                    return 0m;
                }
                var t2 = (changeInLength / (decimal)(alpha * initialLengthInMeter)) + (decimal)initialTempInKelvin;
                return t2;  //Returning final temperature,when final temperature is unknown
            }
            if (initialTempInKelvin.Equals(0d))  //inital Temperature is unknown
            {
                if (finalTempInKelvin.Equals(0d) && alpha.Equals(0d) && changeInLength.Equals(0m) && initialLengthInMeter.Equals(0m))
                {
                    return 0m;
                }
                var t1 = ((changeInLength / (decimal)(alpha * initialLengthInMeter)) + (decimal)finalTempInKelvin) -1;
                return t1;  //Returning inital temperature,when initial tempearature is unknown
            }
            if (alpha.Equals(0d))  //linear Expansivity is unknown 
            {
                if (initialLengthInMeter.Equals(0d) && changeInLength.Equals(0m) && changeInTemperature.Equals(0m))
                {
                    return 0m;
                }
                var linearExpansivity = changeInLength / ((decimal)initialLengthInMeter * changeInTemperature);  //Alpha(Linear Expansivity)
                return linearExpansivity; // Returning Linear Expansivity,when linear expansivity is unknown
            }
            return 0m;
         
        }

        public decimal LinearExpansivityOfTwoDifferentMaterialsAtSameTemprature(double linearExpansivityOfMaterial_A = 0 , double linearExpansivityOfMaterial_B = 0, double lengthOfMaterial_A_In_Meter = 0, double lengthOfMaterial_B_In_Meter = 0)
        {
            if(lengthOfMaterial_B_In_Meter.Equals(0d) && lengthOfMaterial_A_In_Meter.Equals(0d) && 
                linearExpansivityOfMaterial_B.Equals(0d) && linearExpansivityOfMaterial_A.Equals(0d))
            {
                return 0m;
            }
            decimal result = 0m;
            if (linearExpansivityOfMaterial_A.Equals(0d))
             {
                if (lengthOfMaterial_B_In_Meter.Equals(0d) || lengthOfMaterial_A_In_Meter.Equals(0d) ||linearExpansivityOfMaterial_B.Equals(0d))
                {
                    return 0m;
                }
                result = (decimal) ((linearExpansivityOfMaterial_B * lengthOfMaterial_B_In_Meter) / lengthOfMaterial_A_In_Meter);  //Linear Expancivity of Material A is unknown
                return result;
             }
            if (linearExpansivityOfMaterial_B.Equals(0d))
            {
                if (lengthOfMaterial_B_In_Meter.Equals(0d) || lengthOfMaterial_A_In_Meter.Equals(0d) || linearExpansivityOfMaterial_A.Equals(0d))
                {
                    return 0m;
                }
                result = (decimal)((linearExpansivityOfMaterial_A * lengthOfMaterial_A_In_Meter) / lengthOfMaterial_B_In_Meter);  //Linear Expancivity of Material B is unknown
                return result;
            }
            if (lengthOfMaterial_A_In_Meter.Equals(0d))
            {
                if (lengthOfMaterial_B_In_Meter.Equals(0d) || linearExpansivityOfMaterial_A.Equals(0d) || linearExpansivityOfMaterial_B.Equals(0d))
                {
                    return 0m;
                }
                result = (decimal)((linearExpansivityOfMaterial_B * lengthOfMaterial_B_In_Meter) / linearExpansivityOfMaterial_A );  //Length of Material A is unknown
                return result;
            }
            if (lengthOfMaterial_B_In_Meter.Equals(0d))
            {
                if (lengthOfMaterial_A_In_Meter.Equals(0d) || linearExpansivityOfMaterial_A.Equals(0d) || linearExpansivityOfMaterial_B.Equals(0d))
                {
                    return 0m;
                }
                result = (decimal)((linearExpansivityOfMaterial_A * lengthOfMaterial_A_In_Meter) / linearExpansivityOfMaterial_B); //Length of Material B is unknown
                return result;
            }
            return 0m;
        
        }
        
    }

}
