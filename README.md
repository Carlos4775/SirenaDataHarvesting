# Sirena Data Harvesting

The "Sirena Data Harvesting" is a web scraping initiative designed to extract valuable data from "La Sirena," a popular retail website. The primary objective of this project is to gather detailed product information, pricing data, and related content from the "La Sirena" website. This data will then be utilized to power a price comparison application, allowing consumers to make informed purchasing decisions and find the best deals on a wide range of products.

## Installation

Before you proceed with the installation and execution of this project, make sure your environment meets the following requirements:

**MongoDB:** The application uses MongoDB as its database. You must have MongoDB installed and running. You can download MongoDB from the official website [here](https://www.mongodb.com/).

**MongoDB Tools:** To facilitate the import of data into MongoDB, you'll need to install MongoDB Tools. These tools include `mongoimport` for importing data from JSON files. You can download MongoDB Tools from the official website [here](https://www.mongodb.com/try/download/database-tools).

**Clone the repository**

Clone the GitHub repository to your local machine using the following command:

   ```bash
   git clone https://github.com/Carlos4775/SirenaDataHarvesting.git
   cd SirenaDataHarvesting
   ```
Import the database structure and data (data is not mandatory because it will be generated with app execution) using `mongoimport` by using folder MongoDBDump located in: 

   ```bash
    \SirenaDataHarvesting\Data\MongoDBDump
   ```
