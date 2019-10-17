# TestProject
Mono test project

Database was created on local machine with SQLEXPRESS.

Queries:

CREATE TABLE VehicleMake(
	Id INT IDENTITY(1,1),
	Name VARCHAR(50),
	Abrv VARCHAR(20),
  CONSTRAINT PK_VehicleMake PRIMARY KEY (Id));
  
CREATE TABLE VehicleModel(
	Id INT IDENTITY(1,1),
	MakeId INT NOT NULL, 
	Name VARCHAR(50),
	Abrv VARCHAR(20),
	CONSTRAINT PK_VehicleModel PRIMARY KEY (Id),
	CONSTRAINT FK_VehicleModel_VehicleMake FOREIGN KEY (MakeId) REFERENCES VehicleMake(Id));

EntityTypes and DbContext were generated with EF Core Power Tools(Reverse Engineer).


<div>
    <p><strong>Summary</strong>: develop a minimalistic application of your choice by following technologies and concepts mentioned above and requirements defined below.</p>
    <br />
    <p><strong>Requirements</strong></p>
    <ul>
        <li>
            Create a database with following elements
            <ul>
                <li>VehicleMake (Id,Name,Abrv) e.g. BMW,Ford,Volkswagen,</li>
                <li>VehicleModel (Id,MakeId,Name,Abrv) e.g. 128,325,X5 (BWM),</li>
            </ul>
        </li>
        <li>
            Create the solution (back-end) with following projects and elements
            <ul>
                <li>
                    Project.Service
                    <ul>
                        <li>EF models for above database tables</li>
                        <li>VehicleService class - CRUD for Make and Model (Sorting, Filtering &amp; Paging)</li>
                    </ul>
                </li>
                <li>
                    Project.MVC
                    <ul>
                        <li>Make administration view (CRUD with Sorting, Filtering &amp; Paging)</li>
                        <li>
                            Model administration view (CRUD with Sorting, Filtering &amp; Paging)
                            <ul>
                                <li>Filtering by Make</li>
                            </ul>
                        </li>
                    </ul>
                </li>
            </ul>
        </li>
    </ul>
    <br />
    <p><strong>Implementation details</strong></p>
    <ul>
        <li>all classes should be abstracted (have interfaces)</li>
        <li>Mapping should be done by using AutoMapper (<a href="http://automapper.org/">http://automapper.org/</a>)</li>
        <li>EF 6, Core or above with Code First approach (EF Power Tools can be used) should be used</li>
        <li>
            MVC project
            <ul>
                <li>return view models rather than EF database models</li>
                <li>return proper Http status codes</li>
            </ul>
        </li>
    </ul>
</div>
