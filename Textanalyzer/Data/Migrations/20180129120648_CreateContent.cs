using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Textanalyzer.Data.Migrations
{
    public partial class CreateContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Texts",
                columns: table => new
                {
                    TextID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Texts", x => x.TextID);
                });

            migrationBuilder.CreateTable(
                name: "Sentences",
                columns: table => new
                {
                    SentenceID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NextID = table.Column<int>(nullable: true),
                    PreviousID = table.Column<int>(nullable: true),
                    TextID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sentences", x => x.SentenceID);
                    table.ForeignKey(
                        name: "FK_Sentences_Sentences_NextID",
                        column: x => x.NextID,
                        principalTable: "Sentences",
                        principalColumn: "SentenceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sentences_Sentences_PreviousID",
                        column: x => x.PreviousID,
                        principalTable: "Sentences",
                        principalColumn: "SentenceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sentences_Texts_TextID",
                        column: x => x.TextID,
                        principalTable: "Texts",
                        principalColumn: "TextID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    WordID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SentenceID = table.Column<int>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.WordID);
                    table.ForeignKey(
                        name: "FK_Words_Sentences_SentenceID",
                        column: x => x.SentenceID,
                        principalTable: "Sentences",
                        principalColumn: "SentenceID",
                        onDelete: ReferentialAction.Restrict);
                });                      

            migrationBuilder.CreateIndex(
                name: "IX_Sentences_NextID",
                table: "Sentences",
                column: "NextID");

            migrationBuilder.CreateIndex(
                name: "IX_Sentences_PreviousID",
                table: "Sentences",
                column: "PreviousID");

            migrationBuilder.CreateIndex(
                name: "IX_Sentences_TextID",
                table: "Sentences",
                column: "TextID");

            migrationBuilder.CreateIndex(
                name: "IX_Words_SentenceID",
                table: "Words",
                column: "SentenceID");           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {            
            migrationBuilder.DropTable(
                name: "Words");

            migrationBuilder.DropTable(
                name: "Sentences");

            migrationBuilder.DropTable(
                name: "Texts");            
        }
    }
}
