import { CommonModule, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { LoaderService } from '../../../../shared/services/loader.service';

@Component({
  selector: 'app-loader',
  standalone: true,
  imports: [
    CommonModule,
    NgIf
  ],
  template: `
    <div class="loader-backdrop" *ngIf="loaderService.isLoading | async">
      <div class="spinner"></div>
    </div>
  `,
  styleUrls: ['./loader.component.scss']
})
export class AppLoaderComponent {
  constructor(public loaderService: LoaderService) {}
}
