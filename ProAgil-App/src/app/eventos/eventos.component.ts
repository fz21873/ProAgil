import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_model/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';




@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css'],
})
export class EventosComponent implements OnInit {

  eventosFiltrados: Evento[];
  eventos: Evento[];
  imagemLargura = 50;
  imagemMargin = 2;
  mostrarImagem = false;
  modalRef: BsModalRef;

  constructor(
     private eventoService: EventoService
    // tslint:disable-next-line:whitespace
    ,private modalService: BsModalService
    ) { }

// tslint:disable-next-line:variable-name
_filtroLista: string;
  get filtroLista(): string{

    return this._filtroLista;
  }

  set filtroLista(value: string){

     this._filtroLista = value;
     this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  // tslint:disable-next-line:typedef
  openModal(template: TemplateRef<any>){

    this.modalRef = this.modalService.show(template);

  }

 // tslint:disable-next-line:typedef
 ngOnInit(){this.getEventos(); }

   // tslint:disable-next-line:typedef
   alternarImagem() { this.mostrarImagem = !this.mostrarImagem; }

   // tslint:disable-next-line:typedef
    getEventos()  {
     this.eventoService.getAllEvento().subscribe(
    // tslint:disable-next-line:variable-name
    (_eventos: Evento[] ) => {
      this.eventos =  _eventos;
      this.eventosFiltrados = this.eventos;
      console.log(_eventos);
    }, error => {
      console.log(error);
    });
  }

  filtrarEventos(filtarPor: string): Evento[] {

    filtarPor = filtarPor.toLocaleLowerCase();
    return this.eventos.filter(
      evento => (evento.tema.toLocaleLowerCase().indexOf(filtarPor) !== -1)
    );
  }

}
