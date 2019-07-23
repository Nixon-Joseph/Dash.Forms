using Dash.Forms.Extensions;

namespace Dash.Forms.Helpers
{
    public class RunHelper
    {
        //https://fitness.stackexchange.com/questions/15608/energy-expenditure-calories-burned-equation-for-running?newreg=77511cabd58a48e785c4ca8c6cc3ad01
        /// <summary>
        /// Calculates more complicated accurate calories
        /// Requires biometrics
        /// </summary>
        /// <param name="weight">Kilos</param>
        /// <param name="heartRate">BPM</param>
        /// <param name="age">Years</param>
        /// <param name="time">Minutes</param>
        /// <param name="isMale"></param>
        /// <returns></returns>
        public static double CalculateCalories(double weight, double heartRate, double age, double time, bool isMale)
        {
            double baseCalc;
            if (isMale == true)
            {
                baseCalc = (age * 0.2017) + (weight * 0.09036) + (heartRate * 0.6309) - 55.0969;
            }
            else
            {
                baseCalc = (age * 0.074) + (weight * 0.05741) + (heartRate * 0.4472) - 20.4022;
            }

            return baseCalc * time / 4.184;
        }

        //https://fitness.stackexchange.com/questions/15608/energy-expenditure-calories-burned-equation-for-running?newreg=77511cabd58a48e785c4ca8c6cc3ad01
        /// <summary>
        /// Calculates simple calories
        /// Requires weight
        /// </summary>
        /// <param name="meters"></param>
        /// <param name="weight">Kilos</param>
        /// <returns></returns>
        public static double CalculateCalories(double meters, double weight)
        {
            return meters.ToKilometers() * weight * 1.036;
        }
    }
}
