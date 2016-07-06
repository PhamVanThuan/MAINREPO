-- Table: "Address"

-- DROP TABLE "Address";

CREATE TABLE "Address"
(
  "Addressid" serial NOT NULL,
  "StreetNumber" integer,
  "Road" text,
  "Area" text,
  "AreaCode" integer,
  CONSTRAINT "Addressid" PRIMARY KEY ("Addressid")
)
WITH (
  OIDS=FALSE
);
ALTER TABLE "Address"
  OWNER TO postgres;
