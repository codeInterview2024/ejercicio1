using System;
using System.Xml;
using System.IO;

string file = File.ReadAllText("transaction.xml");

if (string.IsNullOrEmpty(file))
{
    Console.WriteLine("Input file is empty");
    return;
}

Console.WriteLine("dumptd.exe - dump network transaction details file");
Console.WriteLine($"CURRENT TRANSACTION DETAILS: \n {file}");

XmlReader reader;

decimal totalAmount = 0;

reader = XmlReader.Create(new StringReader(file));
bool keepGoing = ProcessTransactionItems(reader, true, ref totalAmount);

reader = XmlReader.Create(new StringReader(file));
keepGoing = keepGoing && ProcessTransactionItems(reader, false, out totalAmount);

if (!keepGoing)
{
    Console.WriteLine("Error in retrieving the Fuel Item values");
}

Console.WriteLine($"TransactionTotalLineItem = {totalAmount}");
Console.ReadLine();

static bool ProcessTransactionItems(XmlReader reader, bool fuelItems, decimal totalAmount)
{
    bool keepGoing = true;
    long count = 0;
    string nodeName;

    string descr = "";
    int quantity = 0;
    decimal price = 0;
    decimal unitPrice = 0;
    int productCode = 0;
    string scanCode = "";
    string scanCodeFormat = "";
    int scanCodeModifier = 0;
    string scanCodeModifierName = "";
    bool discountable = false;
    string entryMethod = "";
    int sellingUnits = 0;
    string plu = "";
    int department = 0;
    string departmentDescr = "";
    string status = "";
    int lineNumber = 0;
    int dispenserNumber = 0;
    int dispenserBuffer = 0;
    bool prepayFlag = false;
    int operatingLevelCode = 0;
    int serviceLevelCode = 0;
    int priceLevelCode = 0;
    string serviceLevelName = "";

    if (fuelItems)
    {
        nodeName = "FuelItem";
    }
    else
    {
        nodeName = "NonFuelItem";
    }

    while (reader.Read())
    {
        if (reader.IsStartElement() && reader.Name.Equals(nodeName))
        {
            count++;
            reader.Read();
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                if (reader.Name.Equals("Description"))
                {
                    descr = reader.ReadString();
                }
                if (reader.Name.Equals("Quantity"))
                {
                    quantity = Convert.ToInt32(reader.ReadString());
                }
                if (reader.Name.Equals("Price"))
                {
                    price = Convert.ToDecimal(reader.ReadString());
                    totalAmount = price;
                }
                if (reader.Name.Equals("UnitPrice"))
                {
                    unitPrice = Convert.ToDecimal(reader.ReadString());
                }
                if (reader.Name.Equals("ProductCode"))
                {
                    productCode = Convert.ToInt32(reader.ReadString());
                }
                if (reader.Name.Equals("ScanCode"))
                {
                    scanCode = reader.ReadString();
                }
                if (reader.Name.Equals("ScanCodeFormat"))
                {
                    scanCodeFormat = reader.ReadString();
                }
                if (reader.Name.Equals("ScanCodeModifier"))
                {
                    scanCodeModifier = Convert.ToInt32(reader.ReadString());
                }
                if (reader.Name.Equals("ScanCodeModifierName"))
                {
                    scanCodeModifierName = reader.ReadString();
                }
                if (reader.Name.Equals("Discountable"))
                {
                    discountable = Convert.ToBoolean(reader.ReadString());
                }
                if (reader.Name.Equals("EntryMethod"))
                {
                    entryMethod = reader.ReadString();
                }
                if (reader.Name.Equals("SellingUnits"))
                {
                    sellingUnits = Convert.ToInt32(reader.ReadString());
                }
                if (reader.Name.Equals("PLU"))
                {
                    plu = reader.ReadString();
                }
                if (reader.Name.Equals("Department"))
                {
                    department = Convert.ToInt32(reader.ReadString());
                }
                if (reader.Name.Equals("DepartmentDescription"))
                {
                    departmentDescr = reader.ReadString();
                }
                if (reader.Name.Equals("Status"))
                {
                    status = reader.ReadString();
                }
                if (reader.Name.Equals("LineNumber"))
                {
                    lineNumber = Convert.ToInt32(reader.ReadString());
                }
                if (reader.Name.Equals("DispenserNumber"))
                {
                    dispenserNumber = Convert.ToInt32(reader.ReadString());
                }
                if (reader.Name.Equals("DispenserBuffer"))
                {
                    dispenserBuffer = Convert.ToInt32(reader.ReadString());
                }
                if (reader.Name.Equals("PrepayFlag"))
                {
                    prepayFlag = Convert.ToBoolean(reader.ReadString());
                }
                if (reader.Name.Equals("OperatingLevelCode"))
                {
                    operatingLevelCode = Convert.ToInt32(reader.ReadString());
                }
                if (reader.Name.Equals("ServiceLevelCode"))
                {
                    serviceLevelCode = Convert.ToInt32(reader.ReadString());
                }
                if (reader.Name.Equals("PriceLevelCode"))
                {
                    priceLevelCode = Convert.ToInt32(reader.ReadString());
                }
                if (reader.Name.Equals("ServiceLevelName"))
                {
                    serviceLevelName = reader.ReadString();
                }

                reader.Read();
            }

            if (fuelItems)
            {
                Console.WriteLine("#  Description             Qty   PC   Unit   Price  D# DB# PF OL  SL  PL");
                Console.WriteLine($"{count}  {descr}             {quantity}   {productCode}   {unitPrice}   {price} {dispenserNumber} {dispenserBuffer} {prepayFlag} {operatingLevelCode}  {serviceLevelName}  {priceLevelCode}");
            }
            else
            {
                Console.WriteLine("#  Description             Qty   PC        Unit   Price");
                Console.WriteLine($"{count}  {descr}             {quantity}   {productCode}        {unitPrice}   {price}");
            }
        }
    }

    if (count == 0)
    {
        if (fuelItems)
        {
            Console.WriteLine("*** No Fuel Items ***");
        }
        else
        {
            Console.WriteLine("*** No NonFuel Items ***");
        }
    }

    return keepGoing;
}
