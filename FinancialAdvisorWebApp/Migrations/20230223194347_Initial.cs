using Microsoft.EntityFrameworkCore.Migrations;

namespace FinancialAdvisorWebApp.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facial_Emotions",
                columns: table => new
                {
                    ID_FACE_EMOTION = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_INVEST = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VERSION = table.Column<int>(type: "int", nullable: false),
                    angry = table.Column<float>(type: "real", nullable: false),
                    disgust = table.Column<float>(type: "real", nullable: false),
                    scared = table.Column<float>(type: "real", nullable: false),
                    happy = table.Column<float>(type: "real", nullable: false),
                    sad = table.Column<float>(type: "real", nullable: false),
                    surprised = table.Column<float>(type: "real", nullable: false),
                    neutral = table.Column<float>(type: "real", nullable: false),
                    deviation = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facial_Emotions", x => x.ID_FACE_EMOTION);
                });

            migrationBuilder.CreateTable(
                name: "Investisseurs",
                columns: table => new
                {
                    ID_INVEST = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LASTNAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RISK = table.Column<float>(type: "real", nullable: false),
                    CODE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investisseurs", x => x.ID_INVEST);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    ID_QUESTION = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QUEST = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CODE_QUESTION = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.ID_QUESTION);
                });

            migrationBuilder.CreateTable(
                name: "Speech_Emotions",
                columns: table => new
                {
                    ID_SPEECH_EMOTION = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_INVEST = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VERSION = table.Column<int>(type: "int", nullable: false),
                    emotion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Speech_Emotions", x => x.ID_SPEECH_EMOTION);
                });

            migrationBuilder.CreateTable(
                name: "Choices",
                columns: table => new
                {
                    ID_CHOIX = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WEIGHT = table.Column<int>(type: "int", nullable: false),
                    CHOIX = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ID_QUESTION = table.Column<int>(type: "int", nullable: false),
                    QUESTIONID_QUESTION = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choices", x => x.ID_CHOIX);
                    table.ForeignKey(
                        name: "FK_Choices_Questions_QUESTIONID_QUESTION",
                        column: x => x.QUESTIONID_QUESTION,
                        principalTable: "Questions",
                        principalColumn: "ID_QUESTION",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Questionnaires",
                columns: table => new
                {
                    ID_QUESTIONNAIRE = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_INVEST = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VERSION = table.Column<int>(type: "int", nullable: false),
                    CODE_CHOIX = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ID_CHOIX = table.Column<int>(type: "int", nullable: false),
                    CHOICEID_CHOIX = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questionnaires", x => x.ID_QUESTIONNAIRE);
                    table.ForeignKey(
                        name: "FK_Questionnaires_Choices_CHOICEID_CHOIX",
                        column: x => x.CHOICEID_CHOIX,
                        principalTable: "Choices",
                        principalColumn: "ID_CHOIX",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QUEST_INVEST",
                columns: table => new
                {
                    ID_INVEST = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ID_QUESTIONNAIRE = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QUEST_INVEST", x => new { x.ID_INVEST, x.ID_QUESTIONNAIRE });
                    table.ForeignKey(
                        name: "FK_QUEST_INVEST_Investisseurs_ID_INVEST",
                        column: x => x.ID_INVEST,
                        principalTable: "Investisseurs",
                        principalColumn: "ID_INVEST",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QUEST_INVEST_Questionnaires_ID_QUESTIONNAIRE",
                        column: x => x.ID_QUESTIONNAIRE,
                        principalTable: "Questionnaires",
                        principalColumn: "ID_QUESTIONNAIRE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Choices_QUESTIONID_QUESTION",
                table: "Choices",
                column: "QUESTIONID_QUESTION");

            migrationBuilder.CreateIndex(
                name: "IX_QUEST_INVEST_ID_QUESTIONNAIRE",
                table: "QUEST_INVEST",
                column: "ID_QUESTIONNAIRE");

            migrationBuilder.CreateIndex(
                name: "IX_Questionnaires_CHOICEID_CHOIX",
                table: "Questionnaires",
                column: "CHOICEID_CHOIX");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Facial_Emotions");

            migrationBuilder.DropTable(
                name: "QUEST_INVEST");

            migrationBuilder.DropTable(
                name: "Speech_Emotions");

            migrationBuilder.DropTable(
                name: "Investisseurs");

            migrationBuilder.DropTable(
                name: "Questionnaires");

            migrationBuilder.DropTable(
                name: "Choices");

            migrationBuilder.DropTable(
                name: "Questions");
        }
    }
}
