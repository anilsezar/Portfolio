﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Portfolio.Infrastructure;

#nullable disable

namespace Portfolio.Infrastructure.Migrations
{
    [DbContext(typeof(PortfolioDbContext))]
    partial class PortfolioDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Portfolio.Domain.Entities.EmailToAdmin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email_address");

                    b.Property<bool>("IsItSentSuccessfully")
                        .HasColumnType("boolean")
                        .HasColumnName("is_it_sent_successfully");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("message");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("subject");

                    b.HasKey("Id")
                        .HasName("pk_email_to_admin");

                    b.ToTable("email_to_admin", null, t =>
                        {
                            t.HasComment("Emails that have been sent to the admin via the website ui");
                        });
                });

            modelBuilder.Entity("Portfolio.Domain.Entities.ImageOfTheDay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AltText")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("alt_text");

                    b.Property<bool>("DoIPreferToDisplayThis")
                        .HasColumnType("boolean")
                        .HasColumnName("do_i_prefer_to_display_this");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image_url");

                    b.Property<int>("Source")
                        .HasColumnType("integer")
                        .HasColumnName("source");

                    b.Property<bool>("UrlWorks")
                        .HasColumnType("boolean")
                        .HasColumnName("url_works");

                    b.HasKey("Id")
                        .HasName("pk_image_of_the_day");

                    b.ToTable("image_of_the_day", null, t =>
                        {
                            t.HasComment("List of daily background images by Bing, NASA, etc. They are great");
                        });
                });

            modelBuilder.Entity("Portfolio.Domain.Entities.RequestLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AcceptLanguage")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("accept_language");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("city");

                    b.Property<string>("ClientIp")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("client_ip");

                    b.Property<string>("Connection")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("connection");

                    b.Property<bool>("CookieEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("cookie_enabled");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("country");

                    b.Property<string>("DeviceMemory")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("device_memory");

                    b.Property<string>("DoNotTrack")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("do_not_track");

                    b.Property<string>("Extras")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("extras");

                    b.Property<string>("HardwareConcurrency")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("hardware_concurrency");

                    b.Property<int>("MaxTouchPoints")
                        .HasColumnType("integer")
                        .HasColumnName("max_touch_points");

                    b.Property<bool>("OnLine")
                        .HasColumnType("boolean")
                        .HasColumnName("on_line");

                    b.Property<string>("Platform")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("platform");

                    b.Property<string>("Referrer")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("referrer");

                    b.Property<string>("Resolution")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("resolution");

                    b.Property<string>("UserAgent")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_agent");

                    b.Property<bool>("Webdriver")
                        .HasColumnType("boolean")
                        .HasColumnName("webdriver");

                    b.HasKey("Id")
                        .HasName("pk_request_log");

                    b.ToTable("request_log", null, t =>
                        {
                            t.HasComment("Accesses to the website");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
