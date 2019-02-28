package com.github.venitryx;

import org.javacord.api.DiscordApi;
import org.javacord.api.DiscordApiBuilder;
import org.javacord.api.entity.Icon;
import org.javacord.api.entity.channel.ServerVoiceChannel;
import org.javacord.api.entity.channel.VoiceChannel;
import org.javacord.api.entity.message.MessageBuilder;
import org.javacord.api.entity.message.MessageDecoration;
import org.javacord.api.entity.message.embed.EmbedBuilder;
import org.javacord.api.entity.server.Server;

import java.awt.*;

public class Main {

    public static void main(String[] args) {
        // Insert your bot's token here
        String token = "MzU0MDM5NTY3MjY5MjMyNjUw.D1YWiA.1LDMPgrqGJh8OOd5bbV4nIyQWQA";

        DiscordApi api = new DiscordApiBuilder().setToken(token).login().join();

        ServerVoiceChannel vc = api.getServerVoiceChannelById("325071561675636736").get();
        Server server = api.getServerById("325071515437760514").get();
        api.addMessageCreateListener(event -> {

            if(event.getMessageAuthor().isYourself())
                return;

            if (event.getMessage().getContent().equals("Ene, join the music channel.")) {
                server.moveYourself(vc);
                event.getChannel().sendMessage("Done!");
            }
            if (event.getMessage().getContent().equals("Ene, leave the channel."))
                event.getChannel().sendMessage("Ok, goodbye mortals.");

            if (event.getMessage().getContent().equalsIgnoreCase("Oof")) {
                event.getChannel().sendMessage("xd");
            }
            if (event.getMessage().getContent().equalsIgnoreCase("lol")) {
                event.getChannel().sendMessage("xd");
            }
            if (event.getMessage().getContent().equalsIgnoreCase("owo")) {
                event.getChannel().sendMessage("what dis?");
            }
            if (event.getMessage().getContent().equalsIgnoreCase("uwu")) {
                event.getChannel().sendMessage("what dis?");
            }
            if (event.getMessage().getContent().equalsIgnoreCase("Ene, what is your pfp?")) {


                Icon icon = server.getMemberById("354039567269232650").get().getAvatar();
                new MessageBuilder()
                        .setEmbed(new EmbedBuilder()
                                .setTitle("Sugoi!")
                                .setDescription("Aren't I a good cybergirl?")
                                .setColor(Color.blue))
                                .addAttachment(icon)
                        .send(event.getChannel());


            }
            if(event.getMessageAuthor().isBotOwner()) return;
            /*
            if (event.getMessage().getContent().equalsIgnoreCase("weeb")) {
                event.getChannel().sendMessage("Takes one to know one. ;)");
            }
            if (event.getMessage().getContent().equalsIgnoreCase("no u")) {
                event.getChannel().sendMessage("no u");
            }
            if (event.getMessage().getContent().equalsIgnoreCase("hax")) {
                event.getChannel().sendMessage("nah, just have a good gaming chair");
            }
            if (event.getMessage().getContent().equalsIgnoreCase("nani?")) {
                event.getChannel().sendMessage("omae wa mou shindeiru");
            }
            if (event.getMessage().getContent().startsWith("lmao")) {
                event.getChannel().sendMessage("ewww, that's my line");
            }
            if (event.getMessage().getContent().equalsIgnoreCase("can you don't")) {
                event.getChannel().sendMessage("idk, can you don't?");
            }
            */
        });
        api.addMessageEditListener(event -> {
            if(event.getNewContent().equalsIgnoreCase("Ene, join the music channel.")) {
                server.moveYourself(vc);
                event.getChannel().sendMessage("Done!");
            }
        });
        // Print the invite url of your bot
        System.out.println("You can invite the bot by using the following url: " + api.createBotInvite());
    }

}
