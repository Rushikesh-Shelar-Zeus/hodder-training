import { Component } from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent {
  projectName = 'Contosso Pizza';

  menuOpen = false;

  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }
}
