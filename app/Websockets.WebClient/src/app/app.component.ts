import { Component, OnInit } from '@angular/core';
import { SocketWrapper } from "app/shared/socket-wrapper";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  private socket: SocketWrapper;

  public receivedMessages: any[] = [];

  public title = 'app works!';

  public ngOnInit(): void {
    this.socket = new SocketWrapper();
    this.initalizeSocketMessages();
  }

  public click(message: string): void {
    this.socket.send("broadcast", "chat", message);
  }

  private initalizeSocketMessages() {
    this.socket.on("connected", (data: any) => {
      this.socket.send("message", "adduser", null);
    });

    this.socket.on("chat", (data: any) => {
      this.receivedMessages.push(data);
      if (this.receivedMessages.length > 5)
        this.receivedMessages.splice(0, 1);
    });
  }
}