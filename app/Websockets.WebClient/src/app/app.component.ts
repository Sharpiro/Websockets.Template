import { Component, OnInit } from '@angular/core';
import { SocketWrapper } from "app/shared/socket-wrapper";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  private socket: SocketWrapper;

  public title = 'app works!';

  public ngOnInit(): void {
    this.socket = new SocketWrapper();
  }

  private initalizeSocketMessages() {
    this.socket.on("connect", (data: any) => {
      console.log("connected????????");
    });
  }
}