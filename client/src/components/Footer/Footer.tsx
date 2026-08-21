import { footerSections } from './footerSections';
import '@/styles/global.css';
import logo from '@/assets/graphics/footer/logo2.jfif';

export default function Footer() {
  return (
    <footer className="bg-gray-800 py-8 text-white">
      <div className="container mx-auto">
        <div className="grid grid-cols-1 gap-8 md:grid-cols-4">
          <div className=" items-center text-center">
            <div className="flex items-center justify-center">
              <img alt="logoOfPolitechnikaPoznanska" className="size-10" src={logo} />{' '}
            </div>
            <h2>PUT - WIKI</h2>
            Nowoczesne wsparcie dla studentów Politechniki Poznańskiej. Wszystkie informacje w
            jednym miejscu.
          </div>

          {footerSections.map((section) => (
            <div key={section.id}>
              <h3 className="flex items-center justify-center">{section.title}:</h3>
              <ul>
                {section.links.map((link, index) => (
                  <li className="flex items-center justify-center" key={link.label + index}>
                    <a href={link.href}> {link.label} </a>
                  </li>
                ))}
              </ul>
            </div>
          ))}
        </div>
      </div>
    </footer>
  );
}
