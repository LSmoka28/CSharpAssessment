using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using CsvHelper;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.CodeDom;

namespace ConsoleProjTemp
{
    class Program
    {
        // TODO: save to text file after close
        // TODO: clean up code and case names

        // set the player and shop bank amount
        static public int shopBank = 1000;
        static public int playerBank = 1000;

        // variable for psuedo count of weapons and armor
        static public int weaponNumber = 0;
        static public int armorNumber = 0;



        static void Main(string[] args)
        {
            // create array of weapon and armor class structs before csv load and list conversion
            Weapon.WeaponStruct[] shopWeapInv;
            Armor.ArmorStruct[] shopArmorInv;

            // using csvhelper load .csv files before list conversion
            using (var reader = new StreamReader("WeaponsSpread1.csv"))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    IEnumerable<Weapon.WeaponStruct> weaps = csv.GetRecords<Weapon.WeaponStruct>();
                    shopWeapInv = weaps.ToArray<Weapon.WeaponStruct>();
                }
            }
            using (var reader = new StreamReader("armor.csv"))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    IEnumerable<Armor.ArmorStruct> suits = csv.GetRecords<Armor.ArmorStruct>();
                    shopArmorInv = suits.ToArray<Armor.ArmorStruct>();
                }
            }

            // conversion of weapon and armor arrays to a list
            List<Weapon.WeaponStruct> weaponList = shopWeapInv.ToList();
            List<Armor.ArmorStruct> armorList = shopArmorInv.ToList();

            // create an empty list to add armor and weapons from shop to player inventory
            List<Weapon.WeaponStruct> myWeaps = new List<Weapon.WeaponStruct>();
            List<Armor.ArmorStruct> myArmors = new List<Armor.ArmorStruct>();


            #region Store Introduction and Player ID
            // store intro message
            Prompt($"Welcome to Fantasy Fanatics!\n" +
                $"I have a wide variety of items once belonging to video games, comics, and movies.\n" +
                $"Dont ask how I got them, just enjoy it while you can.\n" +
                $"Dont hesitate to ask for 'help' if you need any assistance!\n");
            Prompt($"First things first,");


            // get player name and location, and check to confirm with player
            bool gettingName = false;
            while (!gettingName)
            {

                // get player name and location
                Prompt($"May I have your name, please? It is for my records...");
                string name = Console.ReadLine().Trim();
                Prompt($"");
                Prompt($"Where are you from?");
                Prompt($"City?");
                string city = Console.ReadLine().Trim();
                Prompt($"State or Country?");
                string stateOrCountry = Console.ReadLine().Trim();
                Prompt($"");

                // add new player after reading player name and location from console
                Player player = new Player(name, new Address(city, stateOrCountry));

                player.Display();
                Prompt($"Correct? ('y' or 'n')");
                if (Console.ReadLine().Trim().ToLower() == "y")
                {
                    Prompt($"");
                    Prompt($"Welcome, {name}!\n");
                    Prompt($"You have {playerBank} units in your bank\n");
                    
                    gettingName = true;
                }
                else
                {
                    Prompt($"Sorry, I must have misheard...");
                }

            }
            #endregion

            #region Main Game Loop
            bool gameRunning = true;
            while (gameRunning)
            {
                Prompt($"What would you like to do?");
                string input = Console.ReadLine().Trim().ToLower();

                // must include 'true' inputCommandDealtWith on each case calling a Show or PlayerInv method
                bool inputCommandDealtWith = false;

                // user input command cases
                switch (input)
                {
                    // shows shop weapon inventory using ShowWeapons method                   
                    case "show weap":
                    case "weapons":
                    case "weap":
                    case "show w":
                        ShowWeapons(weaponList);
                        inputCommandDealtWith = true;
                        break;

                    // shows shop armor inventory using ShowArmor method
                    case "show armor":
                    case "armor":
                    case "arm":
                    case "show a":
                        ShowArmor(armorList);
                        inputCommandDealtWith = true;
                        break;

                    // shows both player inventories 
                    case "inventory":
                    case "my bag":
                    case "bag":
                    case "my inv":
                    case "inv":
                    case "i":
                        PlayerWeaponInv(myWeaps);
                        PlayerArmorInv(myArmors);
                        Prompt($"Bank: {playerBank} units");
                        inputCommandDealtWith = true;
                        break;

                    // sell weapon or armor using w1-w10 or a1-a10 after initial command                    
                    // includes check for player selection out of list range
                    case "sell":
                    case "trade":
                        Prompt($"Pick an item to sell (w1-w10 or a1-a10)");
                        input = Console.ReadLine().Trim().ToLower();
                        char sellInput = input[0];
                        switch (sellInput)
                        {
                            // selects weapon in player inventory based on number
                            case 'w':
                                int tmpWNum;
                                input = input.Replace('w', ' ');
                                int.TryParse(input, out tmpWNum);
                                Prompt($"\nYou chose to sell weapon number {tmpWNum}");
                                try
                                {
                                    SellWeap(myWeaps, weaponList, tmpWNum);
                                }
                                catch (ArgumentOutOfRangeException)
                                {

                                    Prompt($"You entered an invalid weapon selection\n" +
                                        $"Please pick again and dont forget to check your inventory to see what numbers correspond to each weapon\n" +
                                        $"_______________");
                                }
                                break;
                            // selects armor in player inv based on number
                            case 'a':
                                input = input.Replace('a', ' ');
                                int tmpANum;
                                int.TryParse(input, out tmpANum);
                                Prompt($"\nYou chose to sell armor number {tmpANum}");
                                try
                                {
                                    SellArmor(myArmors, armorList, tmpANum);
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    Prompt($"You entered an invalid armor selection\n" +
                                       $"Please pick again and dont forget to check your inventory to see what numbers correspond to each armor\n" +
                                       $"_______________");
                                }
                                break;                       
                        }
                        break;


                    // shows both shop inventories
                    case "shop inv":
                    case "shop":
                    case "show":
                    case "look":
                        ShowWeapons(weaponList);
                        ShowArmor(armorList);
                        inputCommandDealtWith = true;
                        break;

                    // help commands and instructions 
                    case "help":
                    case "commands":
                    case "inputs":
                    case "h":
                        Prompt($"Use the valid commands below to shop the store\nRemember to type 'help' if you get stuck");
                        Prompt($"__________________\nThe current valid commands you can type in are: \n" +
                            $"- y - confirm selection \n" +
                            $"- n - decline selection \n" +
                            $"- w1-w10 - use w and a corresponding number 1-10, to view a weapon in that slot\n" +
                            $"- a1-a10 - use a and a corresponding number 1-10, to view an armor in that slot\n" +
                            $"- load - loads the last saved shop inv and a user entered .csv file for player weapon and armor inventory\n" +
                            $"- show armor, show a, armor, arm  - shows current shop armor inventory and prices \n" +
                            $"- weapons, show weap, show w, weap - shows current shop weapon inventory and prices \n" +
                            $"- inventory, my bag, bag, my inv, inv, i - shows current player inventory \n" +
                            $"- shop inv, shop, show, look - shows the full available shop inventory and prices\n" +
                            $"- sell, trade - sell items back to the shop. for a weapon: type (w1-w10) or an armor: type (a1-a10) \n" +
                            $"- commands, inputs, help, h  - shows the help screen and game instructions \n" +
                            $"- esc, quit, leave, save - asks the user to save files or quit without saving\n");
                        break;

                    // quit and save
                    // asks user to input a name for the weapon inv and armor inv save files
                    case "esc":
                    case "quit":
                    case "leave":
                    case "save":
                        Prompt($"\nThank you for shopping with us! You ended with {playerBank} units in your bank, " +
                            $"{myWeaps.Count} weapon(s), and {myArmors.Count} piece(s) of armor.\n" +
                            $"Would you like to save your progress?");
                        Prompt($"Enter 'y' if you would like to save your inventories\nEnter 'n' if you would like to quit without saving");
                        string saveOrQuit = Console.ReadLine().ToLower().Trim();
                        switch (saveOrQuit)
                        {
                            case"y":
                                {
                                    Prompt($"Enter a name for your weapon save file\nLETTERS(a-z) and/or NUMBERS(0-9) only, no spaces or special characters\n");
                                    string saveWeapName = Console.ReadLine();

                                    using (var writer = new StreamWriter($"{saveWeapName}.csv"))
                                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                                    {
                                        csv.WriteRecords(myWeaps);
                                    }
                                    Prompt($"...");
                                    Prompt($"Weapons file saved as {saveWeapName}.csv\n");
                                }
                                {
                                    Prompt($"Enter a name for your armor save file\nLETTERS(a-z) and/or NUMBERS(0-9) only, no spaces or special characters\n");
                                    string saveArmorName = Console.ReadLine();

                                    using (var writer = new StreamWriter($"{saveArmorName}.csv"))
                                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                                    {
                                        csv.WriteRecords(myArmors);
                                    }
                                    Prompt($"...");
                                    Prompt($"Armor file saved as {saveArmorName}.csv\n");
                                }
                                {
                                    using (var writer = new StreamWriter("ShopArmorSave.csv"))
                                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                                    {
                                        csv.WriteRecords(armorList);
                                    }
                                    Prompt($"...");
                                    Prompt($"Shop armor file saved as ShopArmorSave.csv\n");
                                }
                                {

                                    using (var writer = new StreamWriter("ShopWeaponSave.csv"))
                                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                                    {
                                        csv.WriteRecords(weaponList);
                                    }
                                    Prompt($"...");
                                    Prompt($"Shop weapon file saved as ShopWeaponSave.csv\n");
                                }
                                Prompt($"Please come again.");
                                gameRunning = false;
                                break;

                            case "n":
                                Prompt($"Please come again.");
                                gameRunning = false;
                                break;                         
                        }
                        break;

                    // loads last saved shop inventory
                    // asks user to enter a load file for player weap and armor inv
                    case "load":
                        bool loading = true;
                        while (loading)
                        {
                            Prompt($"What weapon save are you trying to load? Just enter the file name, no extension required\n");
                            {
                                int numberOfTries = 0;
                                int maxNumOfTries = 3;
                                bool weapLoad = true;
                                while (weapLoad)
                                {
                                    string loadWeap = Console.ReadLine().Trim();
                                    try
                                    {
                                        using (var reader = new StreamReader($"{loadWeap}.csv"))
                                        {
                                            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                                            {
                                                myWeaps = csv.GetRecords<Weapon.WeaponStruct>().ToList<Weapon.WeaponStruct>();
                                            }
                                        }
                                        Prompt($"{loadWeap} loaded successfully");
                                        weapLoad = false;
                                        loading = false;
                                    }
                                    catch (HeaderValidationException)
                                    {
                                        if (++numberOfTries == maxNumOfTries)
                                        {
                                            Prompt($"You have reached the maximun amount of tries to load your inventory\nPlease check your inventory to make sure there is a file to load.");
                                            weapLoad = false;
                                            loading = false;
                                            
                                            
                                        }
                                        Prompt($"You may have tried loading the wrong inventory file\nMake sure you are loading your WEAPON save file");
                                    }
                                    catch (FileNotFoundException)
                                    {
                                        Prompt($"\n*File Not Found*");
                                        Prompt($"Check the weapon save file name and try again\n");
                                        if (++numberOfTries == maxNumOfTries)
                                        {
                                            Prompt($"You have reached the maximun amount of tries to load your inventory\nPlease check your inventory to make sure there is a file to load.");
                                            
                                            
                                            break;
                                        }
                                        
                                    }
                                }
                                
                            }
                            

                            Prompt($"What armor save are you trying to load? Just enter the file name, no extension required\n");
                            {
                                int numberOfTries = 0;
                                int maxNumOfTries = 3;
                                bool armorLoad = true;
                                while (armorLoad)
                                {
                                    string loadArmor = Console.ReadLine().Trim();

                                    try
                                    {
                                        using (var reader = new StreamReader($"{loadArmor}.csv"))
                                        {
                                            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                                            {
                                                myArmors = csv.GetRecords<Armor.ArmorStruct>().ToList<Armor.ArmorStruct>();
                                            }
                                        }
                                        Prompt($"{loadArmor} loaded successfully");
                                        armorLoad = false;
                                        loading = false;
                                    }
                                    catch (HeaderValidationException)
                                    {
                                        if (++numberOfTries == maxNumOfTries)
                                        {
                                            Prompt($"You have reached the maximun amount of tries to load your inventory\nPlease check your inventory to make sure there is a file to load.");
                                            armorLoad = false;
                                            loading = false;
                                        }
                                        Prompt($"You may have tried loading the wrong file\nMake sure you are loading your ARMOR save file");
                                    }
                                    catch (FileNotFoundException)
                                    {
                                        Prompt($"*File Not Found*");
                                        Prompt($"Check the armor save file name and try again\n");
                                        if (++numberOfTries == maxNumOfTries)
                                        {
                                            Prompt($"You have reached the maximun amount of tries to load your inventory\nPlease check your inventory to make sure there is a file to load.");
                                            armorLoad = false;
                                            loading = false;
                                        }
                                    }
                                    
                                }

                            }
                            
                            if (loading == true)
                            {

                            }
                            // loads last shop weapon inv save
                            {
                                using (var reader = new StreamReader("ShopWeaponSave.csv"))
                                {
                                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                                    {
                                        weaponList = csv.GetRecords<Weapon.WeaponStruct>().ToList<Weapon.WeaponStruct>();
                                    }
                                }
                            }
                            // loads last shop armor inv save
                            {
                                using (var reader = new StreamReader("ShopArmorSave.csv"))
                                {
                                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                                    {
                                        armorList = csv.GetRecords<Armor.ArmorStruct>().ToList<Armor.ArmorStruct>();
                                    }
                                }
                            }
                            Prompt($"\nLoaded ShopArmorSave and ShopWeaponSave to Shop Inventories\n");
                        }
                        break;
                    

                }
                // gets 'a' or 'w' for choosing an item to view
                // removes 'a' or 'w' to get number associated with proper inventory
                if (!inputCommandDealtWith)
                {
                    char firstLetter = input[0];


                    switch (firstLetter)
                    {

                        case 'w':
                            int tmpWNum;
                            input = input.Replace('w', ' ');
                            int.TryParse(input, out tmpWNum);
                            Prompt($"\nYou chose to view weapon number {tmpWNum}");
                            try
                            {
                                ViewAndBuyWeapon(weaponList, myWeaps, tmpWNum);
                            }
                            catch (ArgumentOutOfRangeException)
                            {

                                Prompt($"You entered an invalid weapon selection\n" +
                                    $"Please pick again and dont forget to check the weapon list to see what numbers correspond to each weapon\n" +
                                    $"_______________");
                            }
                            break;

                        case 'a':
                            input = input.Replace('a', ' ');
                            int tmpANum;
                            int.TryParse(input, out tmpANum);
                            Prompt($"\nYou chose to view armor number {tmpANum}");
                            try
                            {
                                ViewAndBuyArmor(armorList, myArmors, tmpANum);
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                Prompt($"You entered an invalid armor selection\n" +
                                   $"Please pick again and dont forget to check the armor list to see what numbers correspond to each armor\n" +
                                   $"_______________");
                            }
                            break;                        
                    }





                }
            }
            Console.ReadKey();
            #endregion


        }

        #region Methods for Showing, Buying and Selling
        // shows all weapon names, prices and numbers
        static public void ShowWeapons(List<Weapon.WeaponStruct> weaponList)
        {

            Prompt($"");
            Prompt($"\n----WEAPONS----\n     vvvvv    \n");
            foreach (Weapon.WeaponStruct tmpWeap in weaponList)
            {

                weaponNumber++;
                Prompt($"(w{weaponNumber}) - {tmpWeap.Name}\n" +
                    $"-{tmpWeap.Price} units-" +
                    $"\n_______________");

            }
            weaponNumber = 0;
            Prompt($"\n     ^^^^^    \n----WEAPONS----");

            if (playerBank <= 0)
            {
                playerBank = 0;
                Prompt($"Uh oh! You don't have any units left..\nPlease sell something or come back when you have some more units!");

            }
        }

        //shows all armor names and numbers
        static public void ShowArmor(List<Armor.ArmorStruct> armorList)
        {
            Prompt($"");
            Prompt($"\n----ARMOR----\n     vvv    \n");
            foreach (Armor.ArmorStruct tmpArmor in armorList)
            {
                armorNumber++;
                Prompt($"(a{armorNumber}) - {tmpArmor.Name}\n" +
                    $"-{tmpArmor.Price} units-" +
                    $"\n_______________");

            }
            armorNumber = 0;
            Prompt($"\n     ^^^    \n----ARMOR----");

            if (playerBank <= 0)
            {
                playerBank = 0;
                Prompt($"Uh oh! You don't have any units left..\nPlease sell something or come back when you have some more units!");

            }
        }

        // method to view a speciic weapon and asks to purchase
        static public void ViewAndBuyWeapon(List<Weapon.WeaponStruct> weaponList, List<Weapon.WeaponStruct> myWeaps, int indexNum)
        {
            Weapon.WeaponStruct tmpWeap = weaponList[indexNum - 1];
            Item weapon = new Weapon();

            weapon.ToString(tmpWeap.Name, tmpWeap.Type, tmpWeap.Info, tmpWeap.AttackPwr, tmpWeap.Rarity, tmpWeap.Price);

            if (playerBank <= 0)
            {
                playerBank = 0;
                Prompt($"Uh oh! You don't have any units left..\nPlease sell something or come back when you have some more units!");
                return;
            }
            Prompt($"Would you like to buy this weapon? ('y' or 'n')");
            string input = Console.ReadLine();
            if (input == "n" || input == "N")
            {
                return;
            }
            if (input != "y")
            {
                Prompt($"Oops! Looks like you entered and invalid response. \nPlease select your desired weapon and try again\n" +
                    $"*Remember*: \nenter 'y' or 'Y' to buy item\nenter 'n' or 'N' to decline\n___________________");
                return;
            }
            if (input == "y" || input == "Y")
            {
                if (playerBank < tmpWeap.Price)
                {

                    Prompt($"Hey! You dont have any enough units left to purchase that!");
                    Prompt($"You have {playerBank} units and this item is {tmpWeap.Price}");
                    return;
                }
                else
                {
                    weaponList.Remove(tmpWeap);
                    myWeaps.Add(tmpWeap);

                    playerBank -= tmpWeap.Price;
                    shopBank += tmpWeap.Price;


                    Prompt($"You now own {tmpWeap.Name}!\n" +
                        $"You currently have {playerBank} units left in your bank\n________________");
                    return;
                }
            }





        }

        // method to view a speciic armor and asks to purchase
        static public void ViewAndBuyArmor(List<Armor.ArmorStruct> armorList, List<Armor.ArmorStruct> myArmors, int indexNum)
        {
            Item armor = new Armor();
            Armor.ArmorStruct tmpArmor = armorList[indexNum - 1];

            armor.ToString(tmpArmor.Name, tmpArmor.Type, tmpArmor.Info, tmpArmor.Defense, tmpArmor.Rarity, tmpArmor.Price);

            if (playerBank <= 0)
            {
                playerBank = 0;
                Prompt($"Uh oh! You don't have any units left..\nPlease sell something or come back when you have some more units!");
                return;
            }

            Prompt($"Would you like to buy this for protection? ('y' or 'n')");
            string input = Console.ReadLine();
            if (input == "n" || input == "N")
            {
                return;
            }
            if (input != "y")
            {
                Prompt($"Oops! Looks like you entered and invalid response. \nPlease select your desired method of defense and try again\n" +
                       $"*Remember*: \nenter 'y' or 'Y' to buy item\nenter 'n' or 'N' to decline\n___________________\n");
                return;
            }
            if (input == "y" || input == "Y")
            {
                if (playerBank < tmpArmor.Price)
                {

                    Prompt($"Hey! You dont have any enough units left to purchase that!");
                    Prompt($"You have {playerBank} units and this item is {tmpArmor.Price}");
                    return;
                }
                else
                {
                    armorList.Remove(tmpArmor);
                    myArmors.Add(tmpArmor);

                    playerBank -= tmpArmor.Price;
                    shopBank += tmpArmor.Price;


                    Prompt($"You now own {tmpArmor.Name}!\n" +
                        $"You currently have {playerBank} units left in your bank\n___________________\n");
                    return;
                }
            }

        }

        // method to sell a specific weapon
        static public void SellWeap(List<Weapon.WeaponStruct> myWeapInv, List<Weapon.WeaponStruct> weaponList, int indexNum)
        {
            Weapon.WeaponStruct tmpWeap = myWeapInv[indexNum - 1];

            Prompt($"____________________\n- {tmpWeap.Name} -\n" +
                $"Type: {tmpWeap.Type}\n" +
                $"Description:\n{tmpWeap.Info}\n" +
                $"Damage: {tmpWeap.AttackPwr} pts\n" +
                $"Rarity: {tmpWeap.Rarity}\n" +
                $"Cost: {tmpWeap.Price} units\n");

            Prompt($"Would you like to sell this item? ('y' or 'n')");
            string input = Console.ReadLine();

            if (input == "n" || input == "N")
            {
                Prompt($"You declined to sell this weapon\n");
                return;
            }
            if (input != "y")
            {
                Prompt($"Oops! Looks like you entered and invalid response. \nPlease try again\n" +
                       $"*Remember*: \nenter 'y' or 'Y' to sell selected item\nenter 'n' or 'N' to decline\n___________________\n");
                return;
            }
            if (input == "y" || input == "Y")
            {
                if (shopBank < tmpWeap.Price)
                {
                    Prompt($"Yikes. I dont have enough units to buy that back.");
                    Prompt($"The item is {tmpWeap.Price} units and I have only have {shopBank} units");
                    Prompt($"Sorry! Maybe if you buy something from me, I will have enough units then");
                    return;
                }
                else
                {
                    myWeapInv.Remove(tmpWeap);
                    weaponList.Add(tmpWeap);

                    playerBank += tmpWeap.Price;
                    shopBank -= tmpWeap.Price;

                    Prompt($"You sold {tmpWeap.Name} for {tmpWeap.Price} units!\n" +
                        $"You now have {playerBank} units in your bank\n___________________\n");
                    Prompt($"The shop bank is now at {shopBank}");
                }
            }
        }

        // method to sell a specific armor
        static public void SellArmor(List<Armor.ArmorStruct> myArmorInv, List<Armor.ArmorStruct> armorList, int indexNum)
        {
            Armor.ArmorStruct tmpArmor = myArmorInv[indexNum - 1];

            Prompt($"____________________\n- {tmpArmor.Name} -\n" +
                $"Type: {tmpArmor.Type}\n" +
                $"Description:\n{tmpArmor.Info}\n" +
                $"Defense: {tmpArmor.Defense} pts\n" +
                $"Rarity: {tmpArmor.Rarity}\n" +
                $"Cost: {tmpArmor.Price} units\n");

            Prompt($"Would you like to sell this item? ('y' or 'n')");
            string input = Console.ReadLine().Trim().ToLower();
            if (input == "n" || input == "N")
            {
                Prompt($"You declined to sell this armor\n");
                return;
            }
            if (input != "y")
            {
                Prompt($"Oops! Looks like you entered and invalid response. \nPlease try again\n" +
                       $"*Remember*: \nenter 'y' or 'Y' to sell selected item\nenter 'n' or 'N' to decline\n___________________\n");
                return;
            }
            if (input == "y" || input == "Y")
            {
                if (shopBank < tmpArmor.Price)
                {

                    Prompt($"Yikes. I dont have enough units to buy that back.");
                    Prompt($"The item is {tmpArmor.Price} units and I have only have {shopBank} ");
                    return;
                }
                else
                {
                    myArmorInv.Remove(tmpArmor);
                    armorList.Add(tmpArmor);

                    playerBank += tmpArmor.Price;
                    shopBank -= tmpArmor.Price;

                    Prompt($"You sold {tmpArmor.Name} for {tmpArmor.Price} units!\n" +
                        $"You now have {playerBank} units in your bank\n___________________\n");
                    Prompt($"The shop bank is now at {shopBank}");
                    return;
                }
            }
        }

        // shows all information of weapons in player inv
        static public void PlayerWeaponInv(List<Weapon.WeaponStruct> myWeaps)
        {
            Item weapon = new Weapon();
            int numOfWeap = 0;
            Prompt($"\n----MY WEAPONS----\n     vvvvvvvv    \n");
            foreach (Weapon.WeaponStruct myInv in myWeaps)
            {
                numOfWeap++;
                Prompt($"-Weapon w{numOfWeap}-");
                weapon.ToString(myInv.Name, myInv.Type, myInv.Info, myInv.AttackPwr, myInv.Rarity, myInv.Price);


            }
            Prompt($"\n     ^^^^^^^^    \n----MY WEAPONS----\n");
        }

        // shows all information of armor in player inv
        static public void PlayerArmorInv(List<Armor.ArmorStruct> myArmors)
        {
            Item armor = new Armor();
            int numOfArmor = 0;
            Prompt($"\n----MY ARMOR----\n     vvvvvvvv    \n");
            foreach (Armor.ArmorStruct myInv in myArmors)
            {
                numOfArmor++;
                Prompt($"-Armor a{numOfArmor}-");
                armor.ToString(myInv.Name, myInv.Type, myInv.Info, myInv.Defense, myInv.Rarity, myInv.Price);
            }           
            Prompt($"\n     ^^^^^^^^    \n----MY ARMOR----\n");
        }
        #endregion

        // simple method for writing prompts
        static public void Prompt(string prompt)
        {
            Console.WriteLine(prompt);
        }

    }


}
