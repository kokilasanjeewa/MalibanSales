using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class TypeofGood : BaseModel
  {
    [Key]
    public int TypeofGoodId { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "The TypeofGoods cannot exceed 50 characters. ")]
    public string TypeofGoods { set; get; }
    [StringLength(100, ErrorMessage = "The GoodsDescription cannot exceed 100 characters. ")]
    public string GoodsDescription { set; get; }

    [Required]
    public string TypeofGoodUid { get; set; } = Guid.NewGuid().ToString();

  }
}
/* 
Baby
Beer, Wine & Spirits
Beverages:  tea, coffee, soda, juice, Kool-Aid, hot chocolate, water, etc.
Bread & Bakery
Breakfast & Cereal
Canned Goods & Soups
Condiments/Spices & Bake
Cookies, Snacks & Candy
Dairy, Eggs & Cheese
Deli & Signature Cafe
Flowers
Frozen Foods
Produce: Fruits & Vegetables
Grains, Pasta & Sides
International Cuisine
Meat & Seafood
Miscellaneous – gift cards/wrap, batteries, etc.
Paper Products – toilet paper, paper towels, tissues, paper plates/cups, etc.
Cleaning Supplies – laundry detergent, dishwashing soap, etc.
Health & Beauty, Personal Care & Pharmacy – pharmacy items, shampoo, toothpaste
Pet Care
Pharmacy
Tobacco
Here is a more complete list:
Carbonated Beverages
Milk
Fresh Bread & Rolls
Salty Snacks
Beer/Ale/Alcoholic Cider
Natural Cheese
Wine
Fz Dinners/Entrees
Cold Cereal
Cigarettes
Yogurt
Rfg Juices/Drinks
Ice Cream/Sherbet
Bottled Water
Soup
Crackers
Cookies
Bottled Juices
Coffee
Luncheon Meats
Breakfast Meats
Rfg Fresh Eggs
Toilet Tissue
Dog Food
Total Chocolate Candy
Fz Pizza
Rfg Salad/Coleslaw
Fz Novelties
Laundry Detergent
Vegetables
Fz Seafood
Spirits/Liquor
Snack Bars/Granola Bars
Baby Formula/Electrolytes
Processed Cheese
Shortening & Oil
Fz/Rfg Poultry
Cat Food
Spices/Seasonings
Dinner Sausage
Paper Towels
Fz Plain Vegetables
Creams/Creamers
Processed Fz/Rfg Poultry
Frankfurters
Pastry/Doughnuts
Sports Drinks
Fz Breakfast Food
Pasta
Spaghetti/Italian Sauce
Pickles/Relish/Olives
Mexican Foods
Canned/Bottled Fruit
Snack Nuts/Seeds/Corn Nuts
Seafood – Ss
Food & Trash Bags
Salad Dressings – Ss
Rfg Side Dishes
Rice
Total Non-Chocolate Candy
Butter
Dry Packaged Dinners
Fz Meat
Margarine/Spreads/Butter Blen
Dough/Biscuit Dough – Rfg
Tea/Coffee Ready-To-Drink
Mayonnaise
Baking Needs
Sugar
Ss Dinners
Cups & Plates
Baking Mixes
Tomato Products
Vitamins
Fz Potatoes/Onions
Rfg Meat
Soap
Bakery Snacks
Diapers
Household Cleaner
Pies & Cakes
Mexican Sauce
Ss Meat & Rfg Ham
Fz Appetizers/Snack Rolls
Dish Detergent
Cream Cheese/Cr Chs Spread
Peanut Butter
Jellies/Jams/Honey
Internal Analgesics
Gravy/Sauce Mixes
Cottage Cheese
Energy Drinks
Aseptic Juices
Gum
Rfg Entrees
Hot Cereal
Lunches – Rfg
Sour Cream
Mustard & Ketchup
All Other Sauces
Baby Food
Cold/Allergy/Sinus Tablets
Toothpaste
Tea – Bags/Loose
Weight Con/Nutrition Liq/Pwd
Fz Bread/Fz Dough
Pet Supplies
Gastrointestinal – Tablets
Facial Tissue
Sanitary Napkins/Tampons
Flour/Meal
Cat/Dog Litter
Dried Fruit
Shampoo
Popcorn/Popcorn Oil
Syrup/Molasses
Deodorant
Drink Mixes
Baking Nuts
Batteries
Air Fresheners
Gelatin/Pudding Prd And Mixes
Foils & Wraps
Desserts -Rfg
Baked Beans/Canned Bread
Fz Desserts/Topping
Skin Care
Blades
English Muffins
Spreads – Rfg
Rfg Whipped Toppings
Canned Juices – Ss
Rfg Dips
Fz Pies
Cleaning Tools/Mops/Brooms
Toaster Pastries/Tarts
Dry Fruit Snacks
Toothbrush/Dental Accesories
Barbeque Sauce
Rfg Teas/Coffee
Charcoal
Sugar Substitutes
Fz Fruit
Asian Food
Paper Napkins
Fabric Softener Liquid
Hair Conditioner
Seafood – Rfg
Moist Towelettes
Juices – Frozen
Dip/Dip Mixes – Ss\Candles
Dry Beans/Vegetables
Mouthwash
Misc. Snacks
Instant Potatoes
Coffee Creamer
Kitchen Storage
Vinegar
Eye/Contact Lens Care Product
Evaporated/Condensed Milk
Milk Flavoring/Cocoa Mixes
Frosting
Fz Prepared Vegetables
Hand & Body Lotion
Hair Coloring
Bleach
Breadcrumbs/Batters
Other Rfg Products
Non-Fruit Drinks
Light Bulbs
Fz Pasta
Laundry Care
Salad Toppings
Smokeless Tobacco
Fz Pot Pies
Dessert Toppings
Foil Pans
Pest Control
Tea – Instant Tea Mixes
Baked Goods – Rfg
Salad Dressing – Rfg
Misc Health Remedies
Adult Incontinence
Rfg Tortlla/Eggrll/Wontn Wrap
Dried Meat Snacks
Sponges & Scouring Pads
Pizza – Rfg
Steak/Worcestershire Sauce
Rug/Upholstery/Fabric Treatmt
Pancake Mixes
Pasta – Rfg
Fz Side Dishes
Stuffing Mixes
First Aid Treatment
First Aid Accessories
Premixed Cocktails/Coolers
Fabric Softener Sheets
Gastrointestinal – Liquid
Hair Styling Gel/Mousse
Cold/Allergy/Sinus Liquids
Cosmetics – Eye
Suntan Products
Firelog/Firestarter/Firewood
Pickles/Relish – Rfg
Disposable Tableware
Rice/Popcorn Cakes
Croutons
Hair Spray/Spritz
Household Cleaner Cloths
Cocktail Mixes
Cough Drops
Cosmetics – Facial
Baby Accessories
Nasal Products
Baby Needs
Hair Accessories
Marshmallows
Foot Care Products
Office Products
Coffee Filters
Cigars
Cosmetics – Nail
Cheesecakes
Feminine Needs
Specialty Nut Butter
Cough Syrup
Shaving Cream
Fz Corn On The Cob
Denture Products
Cotton Balls/Swabs
All Other Breakfast Food
Pizza Products
Socks
Shaving Lotion/Mens Fragrance
Ice Cream Cones/Mixes
Razors
Childrens Art Supplies
Cosmetics – Lip
Lighters
Furniture Polish
Water Softeners/Treatment
Other Frozen Foods
Writing Instruments
Automobile Fluids/Antifreeze
Contraceptives
External Analgesic Rubs
Gloves
Charcoal Lighter Fluids
Floor Cleaners/Wax Removers
Anti-Smoking Products
Sleeping Remedies
Water Filters/Devices
All Other Tobacco Products
Motor Oil
Family Planning
Pantyhose/Nylons
Fragrances – Women’s
Baking Cups/Paper
*/