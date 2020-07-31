import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_model/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal/';
import { FormGroup,  Validators, FormBuilder } from '@angular/forms';
import { defineLocale, ptBrLocale} from 'ngx-bootstrap/chronos';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
defineLocale('pt-br', ptBrLocale);




@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css'],
})
export class EventosComponent implements OnInit {
  eventosFiltrados: Evento[];
  eventos: Evento[];
  evento: Evento;
  modoSalvar = 'post';
  imagemLargura = 50;
  imagemMargin = 2;
  mostrarImagem = false;
  registerForm: FormGroup;
  bodyDeletarEvento = '';
  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private fb: FormBuilder,
    private localeService: BsLocaleService
    ) {
      this.localeService.use('pt-br');
    }

    // tslint:disable-next-line:variable-name
    _filtroLista: string;
    get filtroLista(): string{

      return this._filtroLista;
    }

    set filtroLista(value: string){

      this._filtroLista = value;
      this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
    }

    openModal(template: any): void{
      this.registerForm.reset();
      template.show();
    }

    editarEvento(evento: Evento, template: any): void {
      this.modoSalvar = 'put';
      this.openModal(template);
      this.evento = evento;
      this.registerForm.patchValue(evento);
    }

    novoEvento(template: any): void{
      this.modoSalvar = 'post';
      this.openModal(template);
    }

    excluirEvento(evento: Evento, template: any): void{
      this.openModal(template);
      this.evento = evento;
      this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${evento.tema}, Código: ${evento.id}`;
    }

    confirmeDelete(template: any): void{
      this.eventoService.deleteEvento(this.evento.id).subscribe(
        () => {
          template.hide();
          this.getEventos();
        }, error => {
          console.log(error);
        }
        );
      }

    // tslint:disable-next-line:typedef
    ngOnInit(){
      this.validation();
      this.getEventos(); }

      alternarImagem(): void { this.mostrarImagem = !this.mostrarImagem; }

      validation(): void{
        this.registerForm = this.fb.group({
          tema:  ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
          local: ['', Validators.required],
          dataEvento: ['', Validators.required],
          qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
          imagemURL: ['', Validators.required],
          telefone: ['', Validators.required],
          email: ['', [Validators.required, Validators.email]]
        });
      }
      salvarAlteracao(template: any): void{

        if (this.registerForm.valid) {
          if (this.modoSalvar === 'post'){
            this.evento = Object.assign({}, this.registerForm.value);
            this.eventoService.postEvento(this.evento).subscribe(
              (novoEvento: Evento) => {
                template.hide();
                this.getEventos();
              }, error => {
                console.log(error);
              }
            );

          } else {

            this.evento = Object.assign({id: this.evento.id}, this.registerForm.value);
            console.log(this.evento.id);
            this.eventoService.putEvento(this.evento).subscribe(
              () => {
                template.hide();
                this.getEventos();
              }, error => {
                console.log(error);
              }
            );

          }
        }
      }
      getEventos(): void  {
        this.eventoService.getAllEvento().subscribe(
          (_EVENTOS: Evento[] ) => {
            this.eventos =  _EVENTOS;
            this.eventosFiltrados = this.eventos;
            console.log(_EVENTOS);
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
