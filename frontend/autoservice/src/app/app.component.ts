import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppLoaderComponent } from './modules/shared/loader/loader/loader.component';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    AppLoaderComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'autoservice';
}
