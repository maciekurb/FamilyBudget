Family Budget Application
=========================

This repository contains a Family Budget application composed of a .NET 6 API backend and an Angular frontend. Each application is containerized using Docker.

Prerequisites
-------------

Before you can run the Family Budget application, you'll need the following software installed on your machine:

*   [Docker](https://www.docker.com/get-started): A platform that uses OS-level virtualization to deliver software in packages called containers.
*   [Docker Compose](https://docs.docker.com/compose/install/): A tool for defining and managing multi-container Docker applications.

Ensure that Docker is installed and running correctly by typing the following command in your terminal:

`docker --version docker-compose --version`

Each of these commands should return the version of Docker and Docker Compose, respectively.

Getting Started
---------------

Here are the steps to get the Family Budget application up and running:

1.  **Clone the Repository**
    
    Start by cloning the repository to your local machine. Open a terminal window, navigate to the directory where you want the project to reside, and run the following command:
    
    `git clone https://github.com/maciekurb/FamilyBudget.git`
        
2.  **Navigate to the Project Directory**
    
    Change your current directory to the `family-budget` directory:
        
    `cd family-budget`
    
3.  **Build and Start the Docker Containers**
    
    Use Docker Compose to build the Docker images and start the Docker containers:
        
    `docker-compose up --build`
    
    This command builds the images for the backend and frontend applications if they haven't been built before and starts the Docker containers. The `--build` flag ensures that Docker Compose rebuilds the images if there have been changes to the Dockerfiles.
    

After running this command, Docker Compose starts all the services defined in the `docker-compose.yml` file. This includes the .NET API, the Angular frontend, and the PostgreSQL database.

You can now access the application at `http://localhost:4200` and the API at `http://localhost:5000`.

Please note that it may take a few minutes for all the services to start up, especially on the first run.

To stop the Docker containers, press `CTRL+C` in your terminal. If you want to completely remove the Docker containers, use the following command:

`docker-compose down`
