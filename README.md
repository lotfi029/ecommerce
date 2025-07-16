E-commerce Application
This is a microservices-based e-commerce application built using .NET, incorporating multiple services to handle various functionalities. Below is an overview of the project, its architecture, services, and setup instructions.
Table of Contents

Project Overview
Architecture
Services
Technologies Used
Prerequisites
Setup Instructions
Running the Application
Contributing
License

Project Overview
This e-commerce application is designed to provide a scalable and modular platform for online shopping. It leverages a microservices architecture, allowing each service to operate independently, communicate asynchronously, and scale as needed. The application supports product management, catalog browsing, order processing, authentication, and inventory tracking.
Architecture
The application follows a microservices architecture with the following components:

Product Service: Manages product information, including creation, updating, and deletion of products.
Catalog Service: Handles product catalog browsing and searching.
Order Service: Processes customer orders and manages order history.
Auth Service: Provides authentication and authorization using Microsoft Identity.
Inventory Service: Tracks product stock levels and updates inventory.
Message Broker (RabbitMQ): Facilitates asynchronous communication between services.
Caching (Redis): Improves performance by caching frequently accessed data.
Databases:
PostgreSQL: Stores relational data for orders and user information.
MongoDB: Stores product and catalog data in a NoSQL format.



Each service is built as a separate .NET application, communicating via REST APIs and RabbitMQ for event-driven interactions.
Services
Product Service

Responsibility: Manages product data (e.g., name, description, price).
Database: MongoDB
Endpoints:
GET /api/products: List all products.
POST /api/products: Create a new product.
PUT /api/products/{id}: Update a product.
DELETE /api/products/{id}: Delete a product.



Catalog Service

Responsibility: Provides product browsing and searching capabilities.
Database: MongoDB
Endpoints:
GET /api/catalog: Retrieve product catalog.
GET /api/catalog/search?query={searchTerm}: Search products by keyword.



Order Service

Responsibility: Handles order creation, updates, and history.
Database: PostgreSQL
Endpoints:
POST /api/orders: Create a new order.
GET /api/orders/{userId}: Get order history for a user.
Events: Publishes OrderCreated event to RabbitMQ.



Auth Service

Responsibility: Manages user authentication and authorization.
Technology: Microsoft Identity (ASP.NET Core Identity with JWT).
Endpoints:
POST /api/auth/register: Register a new user.
POST /api/auth/login: Authenticate a user and return a JWT token.



Inventory Service

Responsibility: Tracks and updates product stock levels.
Database: PostgreSQL
Endpoints:
GET /api/inventory/{productId}: Check stock for a product.
PUT /api/inventory/{productId}: Update stock levels.
Events: Subscribes to OrderCreated event to update inventory.



Technologies Used

Framework: .NET 8 (ASP.NET Core for APIs)
Databases:
PostgreSQL (Relational database for orders and inventory)
MongoDB (NoSQL database for products and catalog)


Message Broker: RabbitMQ (for event-driven communication)
Caching: Redis (for caching product and catalog data)
Authentication: Microsoft Identity (JWT-based authentication)
Containerization: Docker (optional, for deployment)
CI/CD: Configurable with GitHub Actions or similar (not included in this repo)

Prerequisites
Ensure you have the following installed:

.NET 8 SDK
PostgreSQL (v14 or higher)
MongoDB (v5 or higher)
RabbitMQ (v3.9 or higher)
Redis (v6 or higher)
Docker (optional, for containerized setup)
A code editor like Visual Studio Code or Visual Studio

Setup Instructions

Clone the Repository:
git clone https://github.com/your-repo/ecommerce-app.git
cd ecommerce-app


Configure Environment Variables:

Create a .env file in the root directory or configure environment variables for each service.

Example .env:
ConnectionStrings__PostgreSQL=Host=localhost;Database=ecommerce;Username=postgres;Password=your_password
ConnectionStrings__MongoDB=mongodb://localhost:27017/ecommerce
RabbitMQ__Host=localhost
RabbitMQ__Username=guest
RabbitMQ__Password=guest
Redis__Connection=localhost:6379
Jwt__Key=your_jwt_secret_key
Jwt__Issuer=your_issuer
Jwt__Audience=your_audience




Install Dependencies:

Navigate to each service directory (e.g., ProductService, OrderService) and run:
dotnet restore




Set Up Databases:

PostgreSQL: Create a database named ecommerce and run the SQL scripts in the scripts/postgres folder to initialize tables.
MongoDB: Ensure MongoDB is running; no manual schema setup is required (handled by the application).


Start RabbitMQ and Redis:

Ensure RabbitMQ and Redis servers are running locally or in Docker:
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
docker run -d --name redis -p 6379:6379 redis




Build and Run Services:

For each service, navigate to its directory and run:
dotnet build
dotnet run





Running the Application

Start all services (Product, Catalog, Order, Auth, Inventory) using dotnet run or deploy using Docker.

Access the Auth Service to register/login and obtain a JWT token.

Use the token to interact with other services via their APIs (e.g., using Postman or cURL).

Example API call to get products:
curl -H "Authorization: Bearer <your_jwt_token>" http://localhost:5001/api/products



Contributing
Contributions are welcome! Please follow these steps:

Fork the repository.
Create a feature branch (git checkout -b feature/your-feature).
Commit your changes (git commit -m 'Add your feature').
Push to the branch (git push origin feature/your-feature).
Open a Pull Request.

License
This project is licensed under the MIT License. See the LICENSE file for details.
