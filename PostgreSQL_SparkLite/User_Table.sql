-- Table: "User"

-- DROP TABLE "User";

CREATE TABLE "User"
(
  "Userid" serial NOT NULL,
  "Name" text,
  "Surname" text,
  "EmailAddress" text,
  "Password" text,
  "Addressid" integer,
  CONSTRAINT "Userid" PRIMARY KEY ("Userid"),
  CONSTRAINT "Addressid" FOREIGN KEY ("Addressid")
      REFERENCES "Address" ("Addressid") MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE "User"
  OWNER TO postgres;

-- Index: "fki_Addressid"

-- DROP INDEX "fki_Addressid";

CREATE INDEX "fki_Addressid"
  ON "User"
  USING btree
  ("Addressid");

