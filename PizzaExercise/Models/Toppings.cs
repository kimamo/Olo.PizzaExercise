using Newtonsoft.Json;

namespace PizzaExercise.Models
{
    public class Toppings
    {
            [JsonProperty(PropertyName = "toppings")]
            public  string[] toppings { get; set; }
        
    }
}
