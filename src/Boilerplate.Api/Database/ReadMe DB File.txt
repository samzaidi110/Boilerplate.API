--First
create database LibraryManagementDB
--then

-- public."ExceptionLog" definition

-- Drop table

-- DROP TABLE public."ExceptionLog";

CREATE TABLE public."ExceptionLog" (
	"IncidentNumber" varchar(255) NOT NULL,
	"Timestamp" timestamp NOT NULL,
	"ExceptionType" varchar(255) NOT NULL,
	"Message" text NOT NULL,
	"StackTrace" text NOT NULL,
	"HttpMethod" varchar(50) NOT NULL,
	"Url" text NOT NULL,
	"Headers" text NOT NULL,
	"Body" text NOT NULL,
	"RequesterIpAddress" varchar(50) NOT NULL,
	"CorrelationId" varchar(255) NOT NULL,
	"CreatedBy" varchar(255) NOT NULL DEFAULT ''::character varying,
	"CreatedOn" timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"ModifiedBy" varchar(255) NOT NULL DEFAULT ''::character varying,
	"ModifiedOn" varchar(255) NOT NULL DEFAULT ''::character varying,
	"RowStateId" int4 NOT NULL DEFAULT 0,
	"Version" int4 NOT NULL DEFAULT 1,
	"Id" uuid NULL DEFAULT gen_random_uuid()
);

-- public."HttpLog" definition

-- Drop table

-- DROP TABLE public."HttpLog";

CREATE TABLE public."HttpLog" (
	"CorrelationId" text NOT NULL,
	"Timestamp" timestamp NOT NULL,
	"HttpMethod" text NOT NULL,
	"Url" text NOT NULL,
	"Headers" text NOT NULL,
	"RequestBody" text NOT NULL,
	"ResponseBody" text NULL,
	"RequesterIpAddress" text NOT NULL,
	"Status" int4 NOT NULL,
	"DurationMilliseconds" int4 NOT NULL,
	"Type" text NOT NULL,
	"CreatedBy" varchar(255) NOT NULL DEFAULT ''::character varying,
	"ModifiedBy" varchar(255) NOT NULL DEFAULT ''::character varying,
	"ModifiedOn" timestamp NULL DEFAULT CURRENT_TIMESTAMP,
	"CreatedOn" timestamp NULL DEFAULT CURRENT_TIMESTAMP,
	"RowStateId" int4 NOT NULL DEFAULT 0,
	"Version" int4 NOT NULL DEFAULT 1,
	"Id" uuid NULL DEFAULT gen_random_uuid()
);


-- public."Authors" definition

-- Drop table

-- DROP TABLE public."Authors";

CREATE TABLE public."Authors" (
	"Id" int4 NOT NULL DEFAULT nextval('"Author_Id_seq"'::regclass),
	"Name" text NOT NULL,
	CONSTRAINT "Author_pkey" PRIMARY KEY ("Id")
);


-- public."Books" definition

-- Drop table

-- DROP TABLE public."Books";

CREATE TABLE public."Books" (
	"Id" int4 NOT NULL DEFAULT nextval('"Book_Id_seq"'::regclass),
	"Title" text NOT NULL,
	"Description" text NULL,
	"PublishedDate" timestamp NOT NULL,
	"AuthorId" int4 NOT NULL,
	CONSTRAINT "Book_pkey" PRIMARY KEY ("Id")
);


-- public."Books" foreign keys

ALTER TABLE public."Books" ADD CONSTRAINT "FK_Book_Author" FOREIGN KEY ("AuthorId") REFERENCES public."Authors"("Id") ON DELETE CASCADE;



-- public."Genres" definition

-- Drop table

-- DROP TABLE public."Genres";

CREATE TABLE public."Genres" (
	"Id" int4 NOT NULL DEFAULT nextval('"Genre_Id_seq"'::regclass),
	"Name" text NOT NULL,
	CONSTRAINT "Genre_pkey" PRIMARY KEY ("Id")
);

-- public."BookGenres" definition

-- Drop table

-- DROP TABLE public."BookGenres";

CREATE TABLE public."BookGenres" (
	"BookId" int4 NOT NULL,
	"GenreId" int4 NOT NULL,
	CONSTRAINT "PK_BookGenre" PRIMARY KEY ("BookId", "GenreId")
);


-- public."BookGenres" foreign keys

ALTER TABLE public."BookGenres" ADD CONSTRAINT "FK_BookGenre_Book" FOREIGN KEY ("BookId") REFERENCES public."Books"("Id") ON DELETE CASCADE;
ALTER TABLE public."BookGenres" ADD CONSTRAINT "FK_BookGenre_Genre" FOREIGN KEY ("GenreId") REFERENCES public."Genres"("Id") ON DELETE CASCADE;



--also update  connection sring in your app setings   for e.g my is 
--"DefaultConnection": "Host=localhost;Port=5432;Database=LibraryManagementDB;Username=postgres;Password=postgres"
