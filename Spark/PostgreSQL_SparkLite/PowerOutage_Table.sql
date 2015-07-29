-- Table: "PowerOutage"

-- DROP TABLE "PowerOutage";

CREATE TABLE "PowerOutage"
(
  "POid" serial NOT NULL,
  "Date" date,
  "Time" time with time zone,
  "Userid" integer,
  "Stage" text,
  CONSTRAINT "POid" PRIMARY KEY ("POid"),
  CONSTRAINT "Userid" FOREIGN KEY ("Userid")
      REFERENCES "User" ("Userid") MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE "PowerOutage"
  OWNER TO postgres;

-- Index: "fki_Userid"

-- DROP INDEX "fki_Userid";

CREATE INDEX "fki_Userid"
  ON "PowerOutage"
  USING btree
  ("Userid");

